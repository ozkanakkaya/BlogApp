﻿using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Username).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Subject).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Content).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        }
    }
}
