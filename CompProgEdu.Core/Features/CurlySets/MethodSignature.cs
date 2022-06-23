using CompProgEdu.Core.Features.TestCases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class MethodSignature : MethodSignatureDto, IAuditableEntity
    {
        public MethodTestCase MethodTestCase { get; set; }
    }

    public class MethodSignatureDto : IAuditableEntity
    {
        public int Id { get; set; }
        public string FullMethodSignature { get; set; }
        public string MethodName { get; set; }
        public string AccessModifier { get; set; }
        public string ReturnType { get; set; }
        public bool IsReference { get; set; }
        public bool IsAsync { get; set; }
        public bool IsVoid { get; set; }
        public bool IsStatic { get; set; }
        public bool IsReadOnly { get; set; }
        public int AssignmentId { get; set; }
        public List<MethodParameter> MethodParameters { get; set; } = new List<MethodParameter>();
        public int? MethodTestCaseId { get; set; }
    }

    public class MethodSignatureEntityConfiguration : IEntityTypeConfiguration<MethodSignature>
    {
        public void Configure(EntityTypeBuilder<MethodSignature> builder)
        {
            builder.ToTable("MethodSignatures", "domain");
            builder.OwnsMany(p => p.MethodParameters, a =>
            {
                a.WithOwner().HasForeignKey("MethodParameterId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });
            builder
                .HasOne(e => e.MethodTestCase)
                .WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class MethodParameter
    {
        public string ParameterType { get; set; }
        public string ParameterName { get; set; }
    }
}
