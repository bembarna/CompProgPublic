using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class ClassSignature : IAuditableEntity
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string FullClassSignature { get; set; }
        public string AccessModifier { get; set; }
        public bool IsStatic { get; set; }
        public bool IsAbstract { get; set; }
        public string ClassName { get; set; }
    }

    public class ClassSignatureEntityConfiguration : IEntityTypeConfiguration<ClassSignature>
    {
        public void Configure(EntityTypeBuilder<ClassSignature> builder)
        {
            builder.ToTable("ClassSignatures", "domain");
        }
    }
}
