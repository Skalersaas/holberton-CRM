﻿using API.BaseControllers;
using Domain.Models.Entities;
using Domain.Models.JsonTemplates;
using Microsoft.AspNetCore.Mvc;
using Persistance.Data.Repositories;
using System.Text.Json;
using Utilities.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType<ApiResponse<Admission>>(StatusCodes.Status200OK)]
    public class AdmissionController(AdmissionManagement management) : CrudController<Admission, AdmissionDTO>(management.Admissions)
    {
        [ProducesResponseType<ApiResponse<IEnumerable<Admission>>>(StatusCodes.Status200OK)]
        public override Task<ObjectResult> GetAll([FromBody] SearchModel model)
        {
            return base.GetAll(model);
        }
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public override async Task<ObjectResult> New([FromBody] AdmissionDTO entity)
        {
            var student = await management.Students.GetBySlugAsync(entity.StudentSlug);
            var user = await management.Users.GetBySlugAsync(entity.UserSlug);
            if (student is null)
                return ResponseGenerator.NotFound("Student with such SLUG was not found");

            if (user is null)
                return ResponseGenerator.NotFound("User with such SLUG was not found");

            entity.StudentGuid = student.Id;
            entity.UserGuid = user.Id;

            return await base.New(entity);
        }

        public override async Task<ObjectResult> Update([FromBody] Admission entity)
        {
            var prev = await _context.GetByIdAsync(entity.Id);
            if (prev == null)
                return ResponseGenerator.NotFound("Admission with such GUID was not found");

            _context.Detach(prev);

            await TrackAndSaveAdmissionChanges(prev, entity);

            await _context.UpdateAsync(entity);

            return ResponseGenerator.Ok(entity);
        }
        [HttpGet("history")]
        public async Task<ObjectResult> History([FromQuery] Guid id)
        {
            var changes = await management.AdmissionChanges.GetAllAsync(new SearchModel()
            {
                Filters = new Dictionary<string, string>()
                {
                    { nameof(AdmissionChange.AdmissionId), id.ToString() }
                }
            });
            return ResponseGenerator.Ok(changes);
        }
        private async Task TrackAndSaveAdmissionChanges(Admission prev, Admission next)
        {
            var changes = new List<ChangeTemplate>();

            var properties = typeof(Admission).GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == nameof(Admission.User) ||
                    property.Name == nameof(Admission.Student) ||
                    property.Name == nameof(Admission.Notes))
                {
                    continue;
                }

                var prevValue = property.GetValue(prev);
                var nextValue = property.GetValue(next);

                if (!Equals(prevValue, nextValue))
                {
                    changes.Add(new ChangeTemplate
                    {
                        Field = property.Name,
                        Prev = prevValue?.ToString(),
                        Next = nextValue?.ToString()
                    });
                }
            }
            var aChange = new AdmissionChange()
            {
                AdmissionId = prev.Id,
                Data = JsonSerializer.SerializeToDocument(changes),
                CreatedTime = DateTime.UtcNow
            };

            await management.AdmissionChanges.CreateAsync(aChange);
        }

        [Consumes("multipart/form-data")]
        [HttpPost("upload-file")]
        [ProducesResponseType<ApiResponse<IEnumerable<Admission>>>(StatusCodes.Status200OK)]
        public async Task<ObjectResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            if (!file.FileName.EndsWith(".xlsx"))
                return BadRequest("Only .xlsx format is allowed.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            var admissions = ExcelParser.ParseFromExcel<Admission>(stream);

            return Ok(ResponseGenerator.Ok(admissions));
        }

    }
}
