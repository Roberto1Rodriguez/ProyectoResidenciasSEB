using Microsoft.EntityFrameworkCore;
using ProyectoResidenciasApi.Data;

namespace ProyectoResidenciasApi.Repositories
{
    public class Repository<T> where T : class
    {
        private Sistem21ResidenciasSebContext context;

        public Repository(Sistem21ResidenciasSebContext Context)
        {
            this.context = Context;
        }
        public DbSet<T> Get()
        {
            return context.Set<T>();
        }
        public T? Get(object v)
        {
            return context.Find<T>(v);
        }

        public object Insert(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
            return entity;
        }

        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
    }
}

