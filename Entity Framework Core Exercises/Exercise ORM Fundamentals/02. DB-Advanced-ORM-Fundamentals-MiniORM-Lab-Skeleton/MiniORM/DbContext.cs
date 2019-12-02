using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
	public abstract class DbContext
    {
        private readonly DatabaseConnection dbConnection;

        private readonly Dictionary<Type,PropertyInfo> dbSetProperties;

        protected DbContext(string connectionString)
        {
            this.dbConnection = new DatabaseConnection(connectionString);

            this.dbSetProperties = DiscoverDbSetProperties();

            using (var connection = new ConnectionManager(dbConnection))
            {
                this.InitializeDbSets();
            }

            this.MapAllRelations();
        }

        private void MapAllRelations()
        {
            foreach (var dbSetProperty in this.dbSetProperties)
            {
                var mapRelationsMethod = typeof(DbContext)
                    .GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetProperty.Key);

                var dbSet = dbSetProperty.Value.GetValue(this);

                mapRelationsMethod.Invoke(this, new object[] { dbSet});
            }
        }

        private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
               where TEntity : class, new ()
        {
            var entityType = typeof(TEntity);

            MapNavigationProperties(dbSet);

            var collections = entityType.GetProperties()
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() ==
                typeof(ICollection<>))
                .ToArray();

            foreach (var collection in collections)
            {

                var collectionType = collection.PropertyType.GetGenericArguments().Single();

                var mapCollectionMethod = typeof(DbContext).GetMethod(nameof(MapCollection),
                    BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(entityType,collectionType);

                mapCollectionMethod.Invoke(this, new object[] { dbSet,collection});
            }
        }

        private void MapCollection<TDbSet, TCollection>(DbSet<TDbSet> dbSet, PropertyInfo collection)
            where TDbSet : class, new ()
        {
            var entityType = typeof(TDbSet);

            var collectionType = typeof(TCollection);

            var primaryKeys = entityType.GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>()).ToArray();

            var primaryKey = primaryKeys.First();

            var foreignKey = entityType.GetProperties()
                .First(pi => pi.HasAttribute<ForeignKeyAttribute>());

            var isManyToMany = primaryKeys.Length >= 2;
            if (isManyToMany)
            {
                primaryKey = collectionType
                    .GetProperties()
                    .First(pi => collectionType
                    .GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name).PropertyType
                    == entityType);
            }

            var navigationDbSet = (DbSet<TCollection>)this.dbSetProperties[collectionType].GetValue(this);

            foreach (var item in collection)
            {

            }

            var navigationEntities = navigationDbSet
                .Where(navigationEntitity => primaryKey
                .GetValue(navigationEntity)
                .Equals())

        }

        private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet) where TEntity : class, new()
        {
            var entityType = typeof(TEntity);

            var foreignKeys = entityType.GetProperties()
                .Where(pi => pi.HasAttribute<ForeignKeyAttribute>());

            foreach (var foreignKey in foreignKeys)
            {
                var navigationPropertyName = foreignKey
                    .GetCustomAttribute<ForeignKeyAttribute>().Name;

                var navigationProperty = entityType.GetProperty(navigationPropertyName);

                var navigationPropertyType = navigationProperty.PropertyType;

                var navigationDbSet = this.dbSetProperties[navigationPropertyType]
                    .GetValue(this);

                var navigationPrimaryKey = navigationProperty.PropertyType.GetProperties()
                    .First(pi => pi.HasAttribute<KeyAttribute>());

                foreach (var entity in dbSet)
                {
                    var foreignKeyValue = foreignKey.GetValue(entity);

                    var navigationPropertyValue = ((IEnumerable<object>)navigationDbSet)
                        .FirstOrDefault(currentNavigationProperty =>
                            navigationPrimaryKey.GetValue(currentNavigationProperty).Equals(foreignKeyValue));

                    navigationProperty.SetValue(entity, navigationPropertyValue);
                }
                

            }
        }

        private void InitializeDbSets()
        {
            var dbSets = this.dbSetProperties.Values.ToArray();

            foreach (var dbSet in this.dbSetProperties)
            {
                var dbSetType = dbSet.Key;
                var dbSetProperty = dbSet.Value;

                var populateDbSetMethod = typeof(DbContext)
                    .GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(dbSetType);

                populateDbSetMethod.Invoke(this, new object[] { dbSetProperty });
            }
        }

        private void PopulateDbSet<T>(PropertyInfo dbSetProperty)
            where T : class, new ()
        {
            var entities = LoadTableEntities<T>();

            var dbSet = new DbSet<T>(entities);

            dbSetProperty.SetValue(this, dbSet);

            ReflectionHelper.ReplaceBackingField(this,dbSetProperty.Name, dbSet);
        }

        private IEnumerable<TEntity> LoadTableEntities<TEntity>()
            where TEntity : class, new()
        {
            var table = typeof(TEntity);
            var columns = GetEntityColumnNames(table);
            var tableName = GetTableName(table);

            var fetchedRows = this.dbConnection.FetchResultSet<TEntity>(tableName, columns);

            return fetchedRows;
        }

        private string[] GetEntityColumnNames(Type table)
        {
            var tableName = GetTableName(table);

            var dbColumns = this.dbConnection.FetchColumnNames(tableName);

            var columns = table.GetProperties()
                .Where(pi => dbColumns.Contains(pi.Name) && AllowedSqlTypes.SqlTypes.Contains(pi.PropertyType))
                .Select(pi => pi.Name)
                .ToArray();

            return columns;
        }

        private string GetTableName(Type table)
        {
            var tableName = ((TableAttribute)table.GetCustomAttributes(typeof(TableAttribute)).SingleOrDefault()).Name;

            if(tableName == null)
            {
                tableName = this.dbSetProperties[table].Name;
            }

            return tableName;
        }


        private Dictionary<Type, PropertyInfo> DiscoverDbSetProperties()
        {
            var dbSets = this.GetType()
                .GetProperties()
                .Where(pi => pi.PropertyType.IsGenericType
                && pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .ToDictionary(pi => pi.PropertyType.GetGenericArguments().First(), pi => pi);

            return dbSets;
        }
    }
}