#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;

using FluentMigrator.Infrastructure;
using FluentMigrator.Model;

namespace FluentMigrator.Expressions
{
    public class InsertDataExpression : IMigrationExpression, ISupportAdditionalFeatures
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }

        public IDictionary<string, object> AdditionalFeatures { get; } = new Dictionary<string, object>();

        public List<InsertionDataDefinition> Rows { get; } = new List<InsertionDataDefinition>();

        public void CollectValidationErrors(ICollection<string> errors)
        {
        }

        public void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }

        public IMigrationExpression Reverse()
        {
            var expression = new DeleteDataExpression
                                {
                                    SchemaName = SchemaName,
                                    TableName = TableName
                                };

            foreach (var row in Rows)
            {
                var dataDefinition = new DeletionDataDefinition();
                dataDefinition.AddRange(row);

                expression.Rows.Add(dataDefinition);
            }

            return expression;
        }

        public void ApplyConventions(IMigrationConventions conventions)
        {
            if (string.IsNullOrEmpty(SchemaName))
            {
                SchemaName = conventions.GetDefaultSchema();
            }
        }
    }
}
