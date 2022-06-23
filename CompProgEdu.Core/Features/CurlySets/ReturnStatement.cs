using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompProgEdu.Core.Features.CurlySets
{
    public class ReturnStatement : IAuditableEntity
    {
        public int Id { get; set; }
        public int ReturnStartIndex { get; set; }
        public int ReturnEndIndex { get; set; }
        public string ReturnSignature { get; set; }
    }
    public class ReturnStatementEntityConfiguration : IEntityTypeConfiguration<ReturnStatement>
    {
        public void Configure(EntityTypeBuilder<ReturnStatement> builder)
        {
            builder.ToTable("ReturnStatements", "domain");
        }
    }
}
