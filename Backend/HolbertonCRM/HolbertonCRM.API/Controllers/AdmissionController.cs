using Microsoft.AspNetCore.Mvc;

namespace HolbertonCRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdmissionController(/*ApplicationContext context*/) : ControllerBase
    {
        //[HttpGet("all")]
        //public ObjectResult All() => Ok(context.GetAllAdmissions());

        //[HttpGet("new")]
        //public ObjectResult New() => Ok("");

        //[HttpPost("add")]
        //public ObjectResult Add([FromBody] Admission admission)
        //{
        //    admission.Id = 0;
        //    admission.Guid = Guid.NewGuid();
        //    foreach (var note in admission.Notes)
        //        note.Id = 0;
        //    context.AddAdmission(admission);
        //    return Created($"{admission.Guid}", admission);
        //}

        //[HttpPatch("{id}/edit")]
        //public ActionResult Create([FromBody] Admission admission, Guid id)
        //{
        //    var adm = context.GetAdmissionById(id);
        //    if (adm == null)
        //        return NotFound(new { response = "Admission with such Id is not found" });

        //    context.UpdateEntity(admission, adm, "Id", "Guid");

        //    return Ok(new
        //    {
        //        status = "success",
        //        message = "Admission updated successfully"
        //    });
        //}
        //[HttpDelete("{id}/delete")]
        //public ActionResult Delete(Guid id)
        //{
        //    if (!context.RemoveAdmission(id))
        //        return NotFound("No admission with such id");
        //    return Ok("ok");
        //}
        //[HttpPost("{id}/note")]
        //public ObjectResult AddNote([FromBody] AdmissionNote note, Guid id)
        //{
        //    var success = context.AddNoteToAdmission(id, note);
        //    if (!success)
        //        return NotFound(new { response = "Admission with such Id is not found" });

        //    return Ok("Note added");
        //}
        //[HttpGet("{id}/history")]
        //public ObjectResult GetHistory(Guid id)
        //{
        //    return Ok(context.GetAdmissionHistory(id));
        //}
    }
}
