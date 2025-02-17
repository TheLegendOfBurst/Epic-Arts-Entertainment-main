﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Epic_Arts_Entertainment.ORM;

public partial class BdEpicArtsEntertainmentContext : DbContext
{
    public BdEpicArtsEntertainmentContext()
    {
    }

    public BdEpicArtsEntertainmentContext(DbContextOptions<BdEpicArtsEntertainmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAgendamento> TbAgendamentos { get; set; }

    public virtual DbSet<TbServico> TbServicos { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    public virtual DbSet<ViewAgendamento> ViewAgendamentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAB204_2\\SQLEXPRESS;Database=Bd_EpicArtsEntertainment;User Id=BurstDavi;Password=Burst742;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAgendamento>(entity =>
        {
            entity.HasKey(e => e.IdAgendamento).HasName("PK_Tb_Atendimento");

            entity.ToTable("Tb_Agendamento");

            entity.Property(e => e.DtHoraAgendamento).HasColumnType("datetime");

            entity.HasOne(d => d.IdServicoNavigation).WithMany(p => p.TbAgendamentos)
                .HasForeignKey(d => d.IdServico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tb_Atendimento_Tb_Servico");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbAgendamentos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tb_Atendimento_Tb_Usuario");
        });

        modelBuilder.Entity<TbServico>(entity =>
        {
            entity.HasKey(e => e.IdServico);

            entity.ToTable("Tb_Servico");

            entity.Property(e => e.TipoServico)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("Tb_Usuario");

            entity.Property(e => e.DataHoraCadastro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ViewAgendamento>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_Agendamento");

            entity.Property(e => e.DtHoraAgendamento).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TipoServico)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
