using ECommerce.Domain.Exceptions;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if(category is null)
                throw new KeyNotFoundException($"Category with Id {request.Id} was not found.");

            var categoryNameExists = await _categoryRepository.GetByNameAsync(request.Name);
            if(categoryNameExists is not null)
                throw new DomainException($"Category with name {request.Name} already exists.");

            category.Update(request.Name, request.Description);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
