using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateBrandCommandHandler(IBrandRepository brandRepository,IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
           var existingBrand = await _brandRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingBrand != null)
            {
                throw new DomainException($"A brand with the name '{request.Name}' already exists.");
            }

            var brand = Brand.Create(request.Name, request.Description);
            await _brandRepository.AddAsync(brand , cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return brand.Id;
        }
    }
}
