//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\Users\ama\source\repos\EjemploQueryLanguage\query.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="queryParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public interface IqueryVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by the <c>queryConJoin</c>
	/// labeled alternative in <see cref="queryParser.query"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitQueryConJoin([NotNull] queryParser.QueryConJoinContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>querySinJoin</c>
	/// labeled alternative in <see cref="queryParser.query"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitQuerySinJoin([NotNull] queryParser.QuerySinJoinContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="queryParser.join"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitJoin([NotNull] queryParser.JoinContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>condicionUnica</c>
	/// labeled alternative in <see cref="queryParser.condicion"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCondicionUnica([NotNull] queryParser.CondicionUnicaContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>condicionMultiple</c>
	/// labeled alternative in <see cref="queryParser.condicion"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCondicionMultiple([NotNull] queryParser.CondicionMultipleContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>criterioNumerico</c>
	/// labeled alternative in <see cref="queryParser.criterio"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCriterioNumerico([NotNull] queryParser.CriterioNumericoContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>criterioAlphanumerico</c>
	/// labeled alternative in <see cref="queryParser.criterio"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCriterioAlphanumerico([NotNull] queryParser.CriterioAlphanumericoContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>criterioJoin</c>
	/// labeled alternative in <see cref="queryParser.criterio"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCriterioJoin([NotNull] queryParser.CriterioJoinContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>tablaSinAlias</c>
	/// labeled alternative in <see cref="queryParser.tabla"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTablaSinAlias([NotNull] queryParser.TablaSinAliasContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>tablaConAlias</c>
	/// labeled alternative in <see cref="queryParser.tabla"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTablaConAlias([NotNull] queryParser.TablaConAliasContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>seleccionUnica</c>
	/// labeled alternative in <see cref="queryParser.seleccion"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSeleccionUnica([NotNull] queryParser.SeleccionUnicaContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>seleccionMultiple</c>
	/// labeled alternative in <see cref="queryParser.seleccion"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSeleccionMultiple([NotNull] queryParser.SeleccionMultipleContext context);
}
