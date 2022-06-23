using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class PrimitiveVariable : PrimitiveVariableDto, IAuditableEntity
    {
        public CurlySet CurlySet { get; set; }
    }

    public class PrimitiveVariableDto
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string VariableType { get; set; }
        public string VariableName { get; set; }
        public string VariableValue { get; set; }
        public string VariableSignature { get; set; }
        public int CurlySetId { get; set; }
    }

    public class PrimitiveVariableMapping : Profile
    {
        public PrimitiveVariableMapping()
        {
            CreateMap<PrimitiveVariable, PrimitiveVariableDto>();
        }
    }

    public class PrimitiveVariableEntityConfiguration : IEntityTypeConfiguration<PrimitiveVariable>
    {
        public void Configure(EntityTypeBuilder<PrimitiveVariable> builder)
        {
            builder.ToTable("PrimitiveVariables", "domain");
            builder
            .HasOne(e => e.CurlySet)
            .WithMany(c => c.PrimitiveVariables);
        }
    }
}
