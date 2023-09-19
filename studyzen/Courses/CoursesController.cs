﻿using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.Courses.Requests;

namespace StudyZen.Courses;

[ApiController]
[Route("[controller]")]
public sealed class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpPost]
    public IActionResult CreateCourse([FromBody] CreateCourseRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));

        var newCourse = _courseService.AddCourse(request);

        return CreatedAtAction(nameof(GetCourse), new { courseId = newCourse.Id }, newCourse);
    }

    [HttpGet]
    [Route("{courseId}")]
    public async Task<IActionResult> GetCourse(int courseId)
    {
        var fetchedCourse = _courseService.GetCourseById(courseId);
        return fetchedCourse != null ? Ok(fetchedCourse) : NotFound();
    }
}