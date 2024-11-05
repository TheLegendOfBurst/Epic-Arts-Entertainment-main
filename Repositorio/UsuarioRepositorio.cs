using Epic_Arts_Entertainment.Models;
using Epic_Arts_Entertainment.ORM;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace Epic_Arts_Entertainment.Repositories
{
    public class UsuarioRepositorio
    {
        private readonly BdEpicArtsEntertainmentContext _context;

        public UsuarioRepositorio(BdEpicArtsEntertainmentContext context)
        {
            _context = context;
        }

        public void Add(UsuarioVM usuario)
        {
            var tbUsuario = new TbUsuario()
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Senha = usuario.Senha,
                TipoUsuario = usuario.TipoUsuario
            };

            _context.TbUsuarios.Add(tbUsuario);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var tbUsuario = _context.TbUsuarios.FirstOrDefault(f => f.IdUsuario == id);

            if (tbUsuario != null)
            {
                _context.TbUsuarios.Remove(tbUsuario);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Usuário não encontrado.");
            }
        }

        public List<UsuarioVM> GetAll()
        {
            List<UsuarioVM> listaUsuarios = new List<UsuarioVM>();
            var listaTb = _context.TbUsuarios.ToList();

            foreach (var item in listaTb)
            {
                var usuario = new UsuarioVM
                {
                    IdUsuario = item.IdUsuario,
                    Nome = item.Nome,
                    Email = item.Email,
                    Telefone = item.Telefone,
                    Senha = item.Senha,
                    TipoUsuario = item.TipoUsuario
                };

                listaUsuarios.Add(usuario);
            }

            return listaUsuarios;
        }

        public UsuarioVM GetById(int id)
        {
            var item = _context.TbUsuarios.FirstOrDefault(f => f.IdUsuario == id);

            if (item == null)
            {
                return null;
            }

            var usuario = new UsuarioVM
            {
                IdUsuario = item.IdUsuario,
                Nome = item.Nome,
                Email = item.Email,
                Telefone = item.Telefone,
                Senha = item.Senha,
                TipoUsuario = item.TipoUsuario
            };

            return usuario;
        }

        public void Update(UsuarioVM usuario)
        {
            var tbUsuario = _context.TbUsuarios.FirstOrDefault(f => f.IdUsuario == usuario.IdUsuario);

            if (tbUsuario != null)
            {
                tbUsuario.Nome = usuario.Nome;
                tbUsuario.Email = usuario.Email;
                tbUsuario.Telefone = usuario.Telefone;
                tbUsuario.Senha = usuario.Senha;
                tbUsuario.TipoUsuario = usuario.TipoUsuario;

                _context.TbUsuarios.Update(tbUsuario);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Usuário não encontrado.");
            }
        }

        public TbUsuario GetByCredentials(string email, string senha)
        {
            return _context.TbUsuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }
    }
}