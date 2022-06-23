using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class PrimitiveStatement : IAuditableEntity
    {
        public int Id { get; set; }
        public int AssignmentId { get; set; }
        public string Statement { get; set; }
        public string StatementSignature { get; set; }
    }
    public class PrimitiveStatementEntityConfiguration : IEntityTypeConfiguration<PrimitiveStatement>
    {
        public void Configure(EntityTypeBuilder<PrimitiveStatement> builder)
        {
            builder.ToTable("PrimitiveStatements", "domain");
        }
    }
}
