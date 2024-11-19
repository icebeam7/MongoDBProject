using Microsoft.AspNetCore.Mvc;

using StudentAPI.Models;
using StudentAPI.Services;

namespace StudentAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase 
{
    private readonly StudentService studentService;

    public StudentController(StudentService studentService) =>
        this.studentService = studentService;

    [HttpGet]
    public async Task<List<Student>> Get() =>
        await studentService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Student>> Get(string id)
    {
        var student = await studentService.GetAsync(id);

        if (student is null)
            return NotFound();
        
        return student;
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<ActionResult> Delete(string id)
    {
        var student = await studentService.GetAsync(id);

        if (student is null)
            return NotFound();
    
        await studentService.RemoveAsync(id);
        return NoContent();
    }

    [HttpPut("{id:length(24)}")]
    public async Task<ActionResult> Update(string id, Student updatedStudent)
    {
        var student = await studentService.GetAsync(id);

        if (student is null)
            return NotFound();
        
        updatedStudent.Id = student.Id;
        await studentService.UpdateAsync(id, updatedStudent);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> Post(Student student)
    {
        await studentService.CreateAsync(student);

        return CreatedAtAction(nameof(Get), 
        new { id = student.Id}, student);
    }
}