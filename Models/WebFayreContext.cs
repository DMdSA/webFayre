using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebFayre.Models;

public partial class WebFayreContext : DbContext
{
    public WebFayreContext()
    {
    }

    public WebFayreContext(DbContextOptions<WebFayreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoriafeira> Categoriafeiras { get; set; }

    public virtual DbSet<Feira> Feiras { get; set; }

    public virtual DbSet<Funcao> Funcaos { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Patrocinador> Patrocinadors { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Promocaofeira> Promocaofeiras { get; set; }

    public virtual DbSet<Stand> Stands { get; set; }

    public virtual DbSet<Standstaff> Standstaffs { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TipoStand> TipoStands { get; set; }

    public virtual DbSet<Utilizador> Utilizadors { get; set; }

    public virtual DbSet<VendaProduto> VendaProdutos { get; set; }

    public virtual DbSet<Vendum> Venda { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-8VUUH6OV; Database=webFayre;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoriafeira>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaFeira).HasName("PK_categoriafeira_id_categoria_feira");

            entity.ToTable("categoriafeira", "webfayre");

            entity.HasIndex(e => e.Descricao, "IX_categoriafeira").IsUnique();

            entity.Property(e => e.IdCategoriaFeira).HasColumnName("id_categoria_feira");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Feira>(entity =>
        {
            entity.HasKey(e => e.IdFeira).HasName("PK_feira_id_feira");

            entity.ToTable("feira", "webfayre");

            entity.Property(e => e.IdFeira).HasColumnName("id_feira");
            entity.Property(e => e.CapacidadeClientes).HasColumnName("capacidade_clientes");
            entity.Property(e => e.DataFim)
                .HasPrecision(0)
                .HasColumnName("data_fim");
            entity.Property(e => e.DataInicio)
                .HasPrecision(0)
                .HasColumnName("data_inicio");
            entity.Property(e => e.Descricao)
                .HasMaxLength(250)
                .HasColumnName("descricao");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FeiraPath)
                .HasMaxLength(150)
                .HasColumnName("feira_path");
            entity.Property(e => e.Morada)
                .HasMaxLength(80)
                .HasColumnName("morada");
            entity.Property(e => e.NStands).HasColumnName("n_stands");
            entity.Property(e => e.Nome)
                .HasMaxLength(75)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(15)
                .HasColumnName("telefone");

            entity.HasMany(d => d.FeiraCategoria1s).WithMany(p => p.Feiras)
                .UsingEntity<Dictionary<string, object>>(
                    "FeiraCategoria",
                    r => r.HasOne<Categoriafeira>().WithMany()
                        .HasForeignKey("feira_categoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("feira_categorias$categoria_feira"),
                    l => l.HasOne<Feira>().WithMany()
                        .HasForeignKey("feira_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("feira_categorias$id_feira"),
                    j =>
                    {
                        j.HasKey("feira_id", "feira_categoria").HasName("PK_feira_categorias_feira_id");
                        j.ToTable("feira_categorias", "webfayre");
                        j.HasIndex(new[] { "feira_categoria" }, "categoria_feira_idx");
                    });

            entity.HasMany(d => d.Patrocinadors).WithMany(p => p.Feiras)
                .UsingEntity<Dictionary<string, object>>(
                    "PatrocinadorFeira",
                    r => r.HasOne<Patrocinador>().WithMany()
                        .HasForeignKey("patrocinador_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("patrocinador_feira$patrocinador_id"),//???
                    l => l.HasOne<Feira>().WithMany()
                        .HasForeignKey("feira_id")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("patrocinador_feira$feira_id"),
                    j =>
                    {
                        j.HasKey("feira_id", "patrocinador_id").HasName("PK_patrocinador_feira_feira_id");
                        j.ToTable("patrocinador_feira", "webfayre");
                        j.HasIndex(new[] { "feira_id" }, "feira_id_idx");
                        j.HasIndex(new[] { "patrocinador_id" }, "patrocinador_id_idx");
                    });
        });

        modelBuilder.Entity<Funcao>(entity =>
        {
            entity.HasKey(e => e.IdFuncao).HasName("PK_funcao_id_funcao");

            entity.ToTable("funcao", "webfayre");

            entity.Property(e => e.IdFuncao).HasColumnName("id_funcao");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.IdFuncionario).HasName("PK_funcionario_id_funcionario");

            entity.ToTable("funcionario", "webfayre");

            entity.HasIndex(e => e.Email, "funcao_funcionario_idx").IsUnique();

            entity.Property(e => e.IdFuncionario).HasColumnName("id_funcionario");
            entity.Property(e => e.CreationDate)
                .HasColumnType("date")
                .HasColumnName("creation_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Funcao).HasColumnName("funcao");
            entity.Property(e => e.FuncionarioPath)
                .HasMaxLength(150)
                .HasColumnName("funcionario_path");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.Password)
                .HasMaxLength(35)
                .HasColumnName("password");
            entity.Property(e => e.Telemovel)
                .HasMaxLength(17)
                .HasColumnName("telemovel");

            entity.HasOne(d => d.FuncaoNavigation).WithMany(p => p.Funcionarios)
                .HasForeignKey(d => d.Funcao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("funcionario$funcao_funcionario");
        });

        modelBuilder.Entity<Patrocinador>(entity =>
        {
            entity.HasKey(e => e.IdPatrocinador).HasName("PK_patrocinador_id_patrocinador");

            entity.ToTable("patrocinador", "webfayre");

            entity.HasIndex(e => e.Email, "IX_patrocinador").IsUnique();

            entity.Property(e => e.IdPatrocinador).HasColumnName("id_patrocinador");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .HasColumnName("descricao");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.IdProduto).HasName("PK_produto_id_produto");

            entity.ToTable("produto", "webfayre");

            entity.HasIndex(e => e.StandId, "stand_id_idx");

            entity.Property(e => e.IdProduto).HasColumnName("id_produto");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
            entity.Property(e => e.ImagemPath)
                .HasMaxLength(150)
                .HasColumnName("imagem_path");
            entity.Property(e => e.Iva)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("iva");
            entity.Property(e => e.Preco)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("preco");
            entity.Property(e => e.StandId).HasColumnName("stand_id");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.Stand).WithMany(p => p.Produtos)
                .HasPrincipalKey(p => p.IdStand)
                .HasForeignKey(d => d.StandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_produto_stand");
        });

        modelBuilder.Entity<Promocaofeira>(entity =>
        {
            entity.HasKey(e => e.IdPromocaoFeira).HasName("PK_promocaofeira_id_promocao_feira");

            entity.ToTable("promocaofeira", "webfayre");

            entity.HasIndex(e => e.IdFuncionario, "id_funcionario_idx");

            entity.HasIndex(e => e.IdUtilizador, "id_utilizador_idx");

            entity.Property(e => e.IdPromocaoFeira).HasColumnName("id_promocao_feira");
            entity.Property(e => e.CapacidadeUtilizadores).HasColumnName("capacidade_utilizadores");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .HasColumnName("descricao");
            entity.Property(e => e.IdFuncionario).HasColumnName("id_funcionario");
            entity.Property(e => e.IdUtilizador).HasColumnName("id_utilizador");
            entity.Property(e => e.NStands).HasColumnName("n_stands");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdFuncionarioNavigation).WithMany(p => p.Promocaofeiras)
                .HasForeignKey(d => d.IdFuncionario)
                .HasConstraintName("FK_promocaofeira_funcionario");

            entity.HasOne(d => d.IdUtilizadorNavigation).WithMany(p => p.Promocaofeiras)
                .HasForeignKey(d => d.IdUtilizador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("promocaofeira$id_utilizador");
        });

        modelBuilder.Entity<Stand>(entity =>
        {
            entity.HasKey(e => new { e.IdStand, e.FeiraId }).HasName("PK_stand_id_stand");

            entity.ToTable("stand", "webfayre");

            entity.HasIndex(e => e.IdStand, "IX_stand").IsUnique();

            entity.HasIndex(e => e.FeiraId, "stand_feira_id_idx");

            entity.HasIndex(e => e.StandTipoId, "stand_tipo_id_idx");

            entity.Property(e => e.IdStand)
                .ValueGeneratedOnAdd()
                .HasColumnName("id_stand");
            entity.Property(e => e.FeiraId).HasColumnName("feira_id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(75)
                .HasColumnName("descricao");
            entity.Property(e => e.Disponibilidade).HasColumnName("disponibilidade");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Morada)
                .HasMaxLength(150)
                .HasColumnName("morada");
            entity.Property(e => e.Nome)
                .HasMaxLength(45)
                .HasColumnName("nome");
            entity.Property(e => e.StandPath)
                .HasMaxLength(150)
                .HasColumnName("stand_path");
            entity.Property(e => e.StandTipoId).HasColumnName("stand_tipo_id");
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .HasColumnName("telefone");

            entity.HasOne(d => d.Feira).WithMany(p => p.Stands)
                .HasForeignKey(d => d.FeiraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stand$stand_feira_id");

            entity.HasOne(d => d.StandTipo).WithMany(p => p.Stands)
                .HasForeignKey(d => d.StandTipoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stand$stand_tipo_id");

            entity.HasMany(d => d.IdPatrocinadors).WithMany(p => p.IdStands)
                .UsingEntity<Dictionary<string, object>>(
                    "StandPatrocinador",
                    r => r.HasOne<Patrocinador>().WithMany()
                        .HasForeignKey("IdPatrocinador")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("stand_patrocinador$stand_patrocinador_id"),
                    l => l.HasOne<Stand>().WithMany()
                        .HasPrincipalKey("IdStand")
                        .HasForeignKey("IdStand")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_stand_patrocinador_stand1"),
                    j =>
                    {
                        j.HasKey("IdStand", "IdPatrocinador").HasName("PK_stand_patrocinador_id_stand");
                        j.ToTable("stand_patrocinador", "webfayre");
                        j.HasIndex(new[] { "IdPatrocinador" }, "stand_patrocinador_id_idx");
                    });
        });

        modelBuilder.Entity<Standstaff>(entity =>
        {
            entity.HasKey(e => new { e.IdStand, e.StaffEmail }).HasName("PK_standstaff_1");

            entity.ToTable("standstaff", "webfayre");

            entity.Property(e => e.IdStand).HasColumnName("id_stand");
            entity.Property(e => e.StaffEmail)
                .HasMaxLength(45)
                .HasColumnName("staff_email");

            entity.HasOne(d => d.IdStandNavigation).WithMany(p => p.Standstaffs)
                .HasPrincipalKey(p => p.IdStand)
                .HasForeignKey(d => d.IdStand)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_standstaff_stand");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("ticket", "webfayre");

            entity.HasIndex(e => e.Id, "IX_ticket_1").IsUnique();

            entity.HasIndex(e => new { e.FeiraId, e.UtilizadorId }, "IX_ticket_2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data)
                .HasColumnType("datetime")
                .HasColumnName("data");
            entity.Property(e => e.FeiraId).HasColumnName("feira_id");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");

            entity.HasOne(d => d.Feira).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FeiraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ticket_feira");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UtilizadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ticket_utilizador");
        });

        modelBuilder.Entity<TipoStand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tipo_stand_id");

            entity.ToTable("tipo_stand", "webfayre");

            entity.HasIndex(e => e.Descricao, "IX_tipo_stand").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(45)
                .HasColumnName("descricao");
        });

        modelBuilder.Entity<Utilizador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_utilizador_id");

            entity.ToTable("utilizador", "webfayre");

            entity.HasIndex(e => e.Email, "utilizador$email_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("date")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Nif)
                .HasMaxLength(10)
                .HasColumnName("nif");
            entity.Property(e => e.Nome)
                .HasMaxLength(75)
                .HasColumnName("nome");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.Porta).HasColumnName("porta");
            entity.Property(e => e.Rua)
                .HasMaxLength(100)
                .HasColumnName("rua");
            entity.Property(e => e.Telemovel)
                .HasMaxLength(15)
                .HasColumnName("telemovel");
            entity.Property(e => e.UtilizadorPath)
                .HasMaxLength(150)
                .HasColumnName("utilizador_path");

            entity.HasMany(d => d.IdFeiras).WithMany(p => p.IdUtilizadors)
                .UsingEntity<Dictionary<string, object>>(
                    "UtilizadorFavoritaFeira",
                    r => r.HasOne<Feira>().WithMany()
                        .HasForeignKey("IdFeira")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_utilizador_favorita_feira_feira"),
                    l => l.HasOne<Utilizador>().WithMany()
                        .HasForeignKey("IdUtilizador")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("utilizador_favorita_feira$favorita_utilizador_id"),
                    j =>
                    {
                        j.HasKey("IdUtilizador", "IdFeira").HasName("PK_utilizador_favorita_feira_id_utilizador");
                        j.ToTable("utilizador_favorita_feira", "webfayre");
                        j.HasIndex(new[] { "IdFeira" }, "feira_id_idx");
                        j.HasIndex(new[] { "IdUtilizador" }, "utilizador_id_idx");
                    });
        });

        modelBuilder.Entity<VendaProduto>(entity =>
        {
            entity.HasKey(e => new { e.VendaId, e.ProdutoId }).HasName("PK_venda_produto_venda_id");

            entity.ToTable("venda_produto", "webfayre");

            entity.HasIndex(e => e.ProdutoId, "produto_id_idx");

            entity.Property(e => e.VendaId).HasColumnName("venda_id");
            entity.Property(e => e.ProdutoId).HasColumnName("produto_id");
            entity.Property(e => e.Preco)
                .HasColumnType("decimal(6, 2)")
                .HasColumnName("preco");
            entity.Property(e => e.Quantidade)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("quantidade");

            entity.HasOne(d => d.Produto).WithMany(p => p.VendaProdutos)
                .HasForeignKey(d => d.ProdutoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("venda_produto$venda_produto_id");

            entity.HasOne(d => d.Venda).WithMany(p => p.VendaProdutos)
                .HasForeignKey(d => d.VendaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("venda_produto$venda_id");
        });

        modelBuilder.Entity<Vendum>(entity =>
        {
            entity.HasKey(e => e.IdVenda).HasName("PK_venda_id_venda");

            entity.ToTable("venda", "webfayre");

            entity.HasIndex(e => e.StandId, "stand_id_idx");

            entity.HasIndex(e => e.UtilizadorId, "utilizador_id_idx");

            entity.Property(e => e.IdVenda).HasColumnName("id_venda");
            entity.Property(e => e.Data)
                .HasColumnType("date")
                .HasColumnName("data");
            entity.Property(e => e.StandId).HasColumnName("stand_id");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("total");
            entity.Property(e => e.UtilizadorId).HasColumnName("utilizador_id");
            entity.Property(e => e.ValorRegateio)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("valor_regateio");

            entity.HasOne(d => d.Stand).WithMany(p => p.Venda)
                .HasPrincipalKey(p => p.IdStand)
                .HasForeignKey(d => d.StandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_venda_stand");

            entity.HasOne(d => d.Utilizador).WithMany(p => p.Venda)
                .HasForeignKey(d => d.UtilizadorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_venda_utilizador");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
