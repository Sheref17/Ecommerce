using ECommerce.Domain.Exceptions;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBrandCommandHandler(IBrandRepository brandRepository , IUnitOfWork unitOfWork)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.id);
            if (brand is null)
                throw new KeyNotFoundException($"Brand With This Id {request.id} Is Not Found.");

            var brandExistname = await _brandRepository.GetByNameAsync(request.name);
            if (brandExistname is not null)
                throw new DomainException($"Brand With This Name {request.name} Is Aleardy Existt.");

            brand.Update(request.name, request.description);
            await _unitOfWork.SaveChangesAsync();



        }
    }
}
