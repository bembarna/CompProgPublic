using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class PropertySignature : PropertySignatureDto, IAuditableEntity
    {
        public CurlySet CurlySet { get; set; }
    }
    public class PropertySignatureDto
    {
        public int Id { get; set; }
        public string PropertyHead { get; set; }
        public int AssignmentId { get; set; }
        public string PropertyType { get; set; }
        public string PropertyName { get; set; }
        public string AccessModifier { get; set; }
        public bool IsStatic { get; set; }
        public string PropertyFunction { get; set; }
        public int CurlySetId { get; set; }
    }

    public class PropertySignatureMapping : Profile
    {
        public PropertySignatureMapping()
        {
            CreateMap<PropertySignature, PropertySignatureDto>();
        }
    }

    public class PropertySignatureEntityConfiguration : IEntityTypeConfiguration<PropertySignature>
    {
        public void Configure(EntityTypeBuilder<PropertySignature> builder)
        {
            builder.ToTable("PropertySignatures", "domain");
            builder
                .HasOne(e => e.CurlySet)
                .WithMany(c => c.PropertySignatures);
        }
    }
}
