using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EjemploQueryLanguage
{
    internal class Query
    {

        Seleccion? seleccion;
        Condicion? condicion;
        Tabla tablaIzquierda;

        Tabla? tablaDerecha;
        TipoJoin? tipoJoin;
        Condicion? condicionJoin;

        public Query(Tabla tabla) => tablaIzquierda = tabla;

        public Query(Tabla tablaIzquierda, Tabla tablaDerecha, TipoJoin tipoJoin, Condicion? condicionJoin = null) 
            => (this.tablaIzquierda, this.tablaDerecha, this.tipoJoin, this.condicionJoin) = (tablaIzquierda, tablaDerecha, tipoJoin, condicionJoin);

        public void AgregarCondicion(Condicion? condicion) => this.condicion = condicion;
        
        public void AgregarSeleccion(Seleccion? seleccion) => this.seleccion = seleccion;

        public string ObtenerPgSql()
        {
            string sql = "";
            sql += "SELECT " + (seleccion?.Texto() ?? "*");
            sql += "\nFROM " + tablaIzquierda.Texto();
            if (tablaDerecha != null)
            {
                switch (tipoJoin)
                {
                    case TipoJoin.Inner:
                        sql += " INNER JOIN " + tablaDerecha?.Texto(); break;
                    case TipoJoin.Left:
                        sql += " LEFT JOIN " + tablaDerecha?.Texto(); break;
                    case TipoJoin.Right:
                        sql += " RIGHT JOIN " + tablaDerecha?.Texto(); break;
                }
            }
            if (condicionJoin != null)
            {
                sql += " ON " + condicionJoin?.Texto();
            }
            if (condicion != null)
            {
                sql += "\nWHERE " + condicion?.Texto();
            }
            
            return sql;
        }

    }

    enum TipoJoin
    {
        Inner, Left, Right
    }

    internal struct Condicion
    {
        public List<Criterio> criterios = new List<Criterio>();
        
        public void AgregarCriterio(Criterio criterio) => criterios.Add(criterio);
        public Condicion() { }

        public string Texto() => string.Join(" AND ", criterios.Select(x => x.Texto()));

    }
    enum CampoTipo
    {
        Numerico, Alfanumerico, Join
    }

    internal struct Criterio
    {
        public string? alias;
        public string campo;
        public string? valor = null;
        public CampoTipo tipo;

        public string? alias2 = null;
        public string? campo2 = null;

        public Criterio(string campo, string valor, CampoTipo tipo, string? alias=null) 
            => (this.campo, this.valor, this.tipo, this.alias) = (campo, valor, tipo, alias);

        public Criterio(string alias1, string campo1, string alias2, string campo2)
            => (this.alias, this.campo, this.alias2, this.campo2, this.tipo) = (alias1, campo1, alias2, campo2, CampoTipo.Join);

        public string Texto() 
        {
            if (tipo == CampoTipo.Numerico)
                return $"{campo} = {valor}";
            else if (tipo == CampoTipo.Alfanumerico) 
                return $"{campo} = {valor}";
            else //tipo == CampoTipo.Join
                return $"{alias}.{campo} = {alias2}.{campo2}";
        } 

    }


    internal struct Seleccion
    {
        public List<string> campos = new List<string>();
        public Seleccion(List<string> campos) => this.campos = campos;
        public Seleccion(string campo) => campos.Add(campo);

        public string Texto() => string.Join(", ", campos);

    }

    internal struct Tabla
    {
        public string nombre;
        public string? alias;
        public Tabla(string nombre, string? alias=null)
            => (this.nombre, this.alias) = (nombre, alias);

        public string Texto() => alias is not null ? $"{nombre} as {alias}" : nombre;
    }



    internal class ConstructorQuery : queryBaseVisitor<object>
    {
        public override object VisitCondicionMultiple([NotNull] queryParser.CondicionMultipleContext context)
        {
            Condicion condicion = new Condicion();
            foreach (var criterio_tree in context.criterio())
                condicion.AgregarCriterio((Criterio)Visit(criterio_tree));
            return condicion;
        }

        public override object VisitCondicionUnica([NotNull] queryParser.CondicionUnicaContext context)
        {
            Condicion condicion = new Condicion();
            Criterio criterio = (Criterio) Visit(context.criterio());
            condicion.AgregarCriterio(criterio);
            return condicion;
        }

        public override object VisitCriterioAlphanumerico([NotNull] queryParser.CriterioAlphanumericoContext context)
        {
            string campo = context.campo.Text;
            string? alias = context.alias?.Text;
            string valor = context.TEXTO().GetText();
            return new Criterio(campo, valor, CampoTipo.Alfanumerico, alias);
        }

        public override object VisitCriterioNumerico([NotNull] queryParser.CriterioNumericoContext context)
        {
            string campo = context.campo.Text;
            string? alias = context.alias?.Text;
            string valor = context.NUM().GetText();
            return new Criterio(campo, valor, CampoTipo.Numerico, alias);
        }

        public override object VisitQueryConJoin([NotNull] queryParser.QueryConJoinContext context)
        {
            Query query = (Query)Visit(context.join());
            if (context.condicion() != null)
            {
                Condicion condicion = (Condicion)Visit(context.condicion());
                query.AgregarCondicion(condicion);
            }
            if (context.seleccion() != null)
            {
                Seleccion seleccion = (Seleccion)Visit(context.seleccion());
                query.AgregarSeleccion(seleccion);
            }

            return query.ObtenerPgSql();
        }

        public override object VisitQuerySinJoin([NotNull] queryParser.QuerySinJoinContext context)
        {
            Query query = new Query((Tabla)Visit(context.tabla()));
            if (context.condicion() != null)
            {
                Condicion condicion = (Condicion)Visit(context.condicion());
                query.AgregarCondicion(condicion);
            }
            if (context.seleccion() != null)
            {
                Seleccion seleccion = (Seleccion)Visit(context.seleccion());
                query.AgregarSeleccion(seleccion);
            }

            return query.ObtenerPgSql();
        }

        public override object VisitJoin([NotNull] queryParser.JoinContext context)
        {
            Tabla tablaIzquierda = (Tabla)Visit(context.tabla1);
            Tabla tablaDerecha = (Tabla)Visit(context.tabla2);
            TipoJoin tipo;
            switch (context.tipo.Type)
            {
                case queryParser.AND:
                    tipo = TipoJoin.Inner;
                    break;
                case queryParser.LT:
                    tipo = TipoJoin.Left;
                    break;
                case queryParser.GT:
                    tipo = TipoJoin.Right;
                    break;
                default:
                    tipo = TipoJoin.Inner;
                    break;
            }

            if (context.condicion() != null)
            {
                Condicion condicionJoin = (Condicion)Visit(context.condicion());
                return new Query(tablaIzquierda, tablaDerecha, TipoJoin.Right, condicionJoin);
            }
            return new Query(tablaIzquierda, tablaDerecha, tipo);
        }

        public override object VisitCriterioJoin([NotNull] queryParser.CriterioJoinContext context)
        {
            string campo1 = context.campo1.Text;
            string alias1 = context.alias1.Text;
            string campo2 = context.campo2.Text;
            string alias2 = context.alias2.Text;
            return new Criterio(alias1,campo1,alias2,campo2);
        }

        public override object VisitSeleccionMultiple([NotNull] queryParser.SeleccionMultipleContext context)
        {
            Seleccion seleccion = new Seleccion(context.ID().Select(x => x.GetText()).ToList());
            return seleccion;
        }

        public override object VisitSeleccionUnica([NotNull] queryParser.SeleccionUnicaContext context)
        {
            Seleccion seleccion = new Seleccion(context.ID().GetText());
            return seleccion;
        }

        public override object VisitTablaConAlias([NotNull] queryParser.TablaConAliasContext context)
        {
            return new Tabla(context.nombre.Text, context.alias.Text);
        }

        public override object VisitTablaSinAlias([NotNull] queryParser.TablaSinAliasContext context)
        {
            return new Tabla(context.nombre.Text);
        }
    }
}//estudiantes(a) <  papapa(s){apellido  = "pepe", papa=3}[  apellido  = "pepe", papa=3 ]   .nombre
