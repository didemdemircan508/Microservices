using FluentValidation;
using FreeCourse.Web.Models.Catalogs;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator:AbstractValidator<CourseCreateInput>
    {

        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Alanı Boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama Alanı Boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1,int.MaxValue).WithMessage("Süre Alanı Boş olamaz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat Alanı Boş olamaz").ScalePrecision(2, 6).WithMessage("hatalı format");//5555.66
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori Alanı Seçiniz");
        }
    }
}
