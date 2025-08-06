using Microsoft.InfoNav.Data.Contracts.Internal;
using Microsoft.InfoNav.Explore.ServiceContracts.Internal;
using System.Collections.Generic;

namespace ConsoleApp3
{
    class Query
    {
        public static DataViewQueryDefinition GetQuery()
        {
            var a_col_expr = new QueryColumnExpression()
            {
                Expression = new QuerySourceRefExpression()
                {
                    Source = "k"
                },
                Property = "a"
            };
            var b_col_expr = new QueryColumnExpression()
            {
                Expression = new QuerySourceRefExpression()
                {
                    Source = "k"
                },
                Property = "b"
            };
            var t_col_expr = new QueryColumnExpression()
            {
                Expression = new QuerySourceRefExpression()
                {
                    Source = "t"
                },
                Property = "b"
            };
            return new DataViewQueryDefinition(queryDefinition: new QueryDefinition()
            {
                Version = 2,
                DatabaseName = "hi",
                From = new List<EntitySource>{
                    new EntitySource() {
                        Entity = "TestData",
                        Name = "k",
                        Type = EntitySourceType.Table
                    },
                    new EntitySource() {
                        Entity = "Table",
                        Name = "t",
                        Type = EntitySourceType.Table
                    },
                },
                Select = new List<QueryExpressionContainer>{
                    new QueryExpressionContainer(
                        expression: new QueryAggregationExpression(){
                            Function = QueryAggregateFunction.Sum,
                            Expression = a_col_expr
                        },
                        name: "Sum(TestData.a)",
                        nativeReferenceName: "a"
                    ),
                    new QueryExpressionContainer(
                        expression: new QueryAggregationExpression(){
                            Function = QueryAggregateFunction.Sum,
                            Expression = b_col_expr
                        },
                        name: "Sum(TestData.b)",
                        nativeReferenceName: "b"
                    )
                },
                Where = new List<QueryFilter> {
                    new QueryFilter(){
                        Condition=new QueryExpressionContainer(
                            expression: new QueryNotExpression(){
                                Expression = new QueryInExpression(){
                                    Expressions = new List<QueryExpressionContainer>{
                                        a_col_expr
                                    },
                                    Values = new List<List<QueryExpressionContainer>>{ new List<QueryExpressionContainer> { new QueryLiteralExpression() { Value = "3L"} } }
                                }
                            }
                        )
                    },
                    new QueryFilter(){
                        Condition=new QueryExpressionContainer(
                            expression: new QueryNotExpression(){
                                Expression = new QueryContainsExpression(){
                                    Left = t_col_expr,
                                    Right = new QueryLiteralExpression(){ Value = "'A'" }
                                }
                            }
                        )
                    }
                },
                OrderBy = new List<QuerySortClause> {
                    new QuerySortClause() {
                        Direction = QuerySortDirection.Descending,
                        Expression = new QueryAggregationExpression(){
                            Function = QueryAggregateFunction.Sum,
                            Expression = a_col_expr
                        }
                    } 
                },

            });
        }
    }
}
