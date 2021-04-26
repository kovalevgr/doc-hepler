﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DocHelper.Application.Common.Mappings;
using DocHelper.Application.Common.Specifications;
using DocHelper.Domain.Dto;
using DocHelper.Domain.Repository;
using DocHelper.Domain.Specification.Doctor;
using MediatR;

namespace DocHelper.Application.Doctor.Command.ListDoctorsWithPagination
{
    public class ListDoctorsWithPaginationCommand : IRequest<IEnumerable<DoctorListDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
    }

    public class
        ListDoctorsWithPaginationCommandHandler : IRequestHandler<ListDoctorsWithPaginationCommand,
            IEnumerable<DoctorListDto>>
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly SpecBuilder<Domain.Entities.DoctorAggregate.Doctor> _builder;

        public ListDoctorsWithPaginationCommandHandler(
            IDoctorRepository doctorRepository,
            IMapper mapper,
            SpecBuilderFactory builder)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _builder = builder.Create<Domain.Entities.DoctorAggregate.Doctor>();
        }


        public async Task<IEnumerable<DoctorListDto>> Handle(
            ListDoctorsWithPaginationCommand request,
            CancellationToken cancellationToken)
        {
            var queryable = _builder
                .AddSpecification(new DoctorListSpecification())
                .Queryable
                .ProjectTo<DoctorListDto>(_mapper.ConfigurationProvider)
                .Paginate(request.PageNumber, request.PageSize);
            
            return await _doctorRepository.ListAsync(queryable, cancellationToken);
        }
    }
}