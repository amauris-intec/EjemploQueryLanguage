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
            return "";
        }

    }

    enum TipoJoin
    {
        Inner, Left, Right
    }

    internal struct Condicion
    {
        public Criterio[] criterios = new Criterio[] { };
        
        public void AgregarCriterio(Criterio criterio) => criterios.Append(criterio);
        public Condicion() { }

    }
    enum CampoTipo
    {
        Numerico, Alfanumerico
    }

    internal struct Criterio
    {
        public string campo;
        public string valor;
        public CampoTipo tipo;
        public Criterio(string campo, string valor, CampoTipo tipo) 
            => (this.campo, this.valor, this.tipo) = (campo, valor, tipo);

    }


    internal struct Seleccion
    {
        public string[] campos = new string[] { };
        public Seleccion() { }
        public void AgregarCampo(string campo) => campos.Append(campo);

    }

    internal struct Tabla
    {
        public string nombre;
        public string? alias;
        public Tabla(string nombre, string? alias=null)
            => (this.nombre, this.alias) = (nombre, alias);
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
            string campo = context.ID().GetText();
            string valor = context.TEXTO().GetText();
            return new Criterio(campo, valor, CampoTipo.Alfanumerico);
        }

        public override object VisitCriterioNumerico([NotNull] queryParser.CriterioNumericoContext context)
        {
            string campo = context.ID().GetText();
            string valor = context.NUM().GetText();
            return new Criterio(campo, valor, CampoTipo.Numerico);
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

        public override string?[] VisitSeleccionMultiple([NotNull] queryParser.SeleccionMultipleContext context)
        {
            return context.ID().Select(x => x.ToString()).ToArray();
        }

        public override string VisitSeleccionUnica([NotNull] queryParser.SeleccionUnicaContext context)
        {
            return context.ID().GetText();
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
