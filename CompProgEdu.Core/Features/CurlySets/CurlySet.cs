using AutoMapper;
using CompProgEdu.Core.Features.Instructors;
using CompProgEdu.Core.Features.Submissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class CurlySet : CurlySetGetDto, IAuditableEntity
    {
        public int OpenCurlyId { get; set; }
        public int? ClosedCurlyId { get; set; }
        public int OpenCurlyPositionInString { get; set; }
        public int CloseCurlyPositionInString { get; set; }
        public List<CurlySet> CurlySets { get; set; } = new List<CurlySet>();
        public CurlySet ParentCurlySet { get; set; }
        public List<PrimitiveVariable> PrimitiveVariables { get; set; } = new List<PrimitiveVariable>();
        public List<PropertySignature> PropertySignatures { get; set; } = new List<PropertySignature>();
    }

    public class CurlySetGetDto : CurlySetDto
    {
        public int Id { get; set; }
    }

    public class CurlySetDto
    {
        public int AssignmentId { get; set; }
        public int? ParentId { get; set; }
        public bool IsMethod { get; set; }
        public bool IsMain { get; set; }
        public bool IsClass { get; set; }
        public int? PropertySignatureId { get; set; }
        public int? ClassSignatureId { get; set; }
        public ClassSignature ClassSignature { get; set; }
        public int? MethodSignatureId { get; set; }
        public MethodSignature MethodSignature { get; set; }
        public int? StatementSignatureId { get; set; }
        public PrimitiveStatement StatementSignature { get; set; }
        public bool IsPrimitiveStatement { get; set; }
        public int? ReturnStatementId { get; set; }
        public ReturnStatement ReturnStatement { get; set; }
        public bool HasReturn { get; set; }
    }

    public class CurlySetDetailDto : CurlySetGetDto
    {
        public List<PrimitiveVariableDto> PrimitiveVariables { get; set; } = new List<PrimitiveVariableDto>();
        public List<PropertySignatureDto> PropertySignatures { get; set; } = new List<PropertySignatureDto>();
        public List<CurlySetDetailDto> CurlySets { get; set; } = new List<CurlySetDetailDto>();
    }

    public class CurlySetMapping : Profile
    {
        public CurlySetMapping()
        {
            CreateMap<CurlySet, CurlySetDto>();
            CreateMap<CurlySet, CurlySetGetDto>();
            CreateMap<CurlySet, CurlySetDetailDto>();
        }
    }

    public class CurlySetEntityConfiguration : IEntityTypeConfiguration<CurlySet>
    {
        public void Configure(EntityTypeBuilder<CurlySet> builder)
        {
            builder.ToTable("CurlySets", "domain");
            builder
            .HasMany(e => e.CurlySets)
            .WithOne(s => s.ParentCurlySet)
            .HasForeignKey(e => e.ParentId);

            builder.HasMany(c => c.PrimitiveVariables)
            .WithOne(e => e.CurlySet);

            builder.HasMany(c => c.PropertySignatures)
            .WithOne(e => e.CurlySet);

            builder.HasOne(x => x.ClassSignature).WithMany();
            builder.HasOne(x => x.MethodSignature).WithMany();
            builder.HasOne(x => x.StatementSignature).WithMany();
            builder.HasOne(x => x.ReturnStatement).WithMany();
        }
    }
}
