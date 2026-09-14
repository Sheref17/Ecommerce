using ECommerce.Domain.Entities;
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
           var brand = Brand.Create(request.Name, request.Description);
            await _brandRepository.AddAsync(brand , cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return brand.Id;
        }
    }
}
