using StudyZen.Domain.Entities;
using StudyZen.Application.Dtos;

namespace StudyZen.DtoControllers
{
    public class CourseDtoController
    {
        public static CourseDto ToDto(Course course)
        {
            return new CourseDto
            {
               Id = course.Id,
               Name = course.Name,
               Description = course.Description
            };
        }
    }
}