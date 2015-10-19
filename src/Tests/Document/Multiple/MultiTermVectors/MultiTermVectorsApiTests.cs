﻿using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Nest;
using Tests.Framework;
using Tests.Framework.Integration;
using Tests.Framework.MockData;
using Xunit;
using static Nest.Static;

namespace Tests.Document.Multiple.MultiTermVectors
{
	[Collection(IntegrationContext.ReadOnly)]
	public class MultiTermVectorsApiTests : ApiTestBase<IMultiTermVectorsResponse, IMultiTermVectorsRequest, MultiTermVectorsDescriptor, MultiTermVectorsRequest>
	{
		public MultiTermVectorsApiTests(ReadOnlyCluster cluster, EndpointUsage usage) : base(cluster, usage) { }
		protected override LazyResponses ClientUsage() => Calls(
			fluent: (client, f) => client.MultiTermVectors(f),
			fluentAsync: (client, f) => client.MultiTermVectorsAsync(f),
			request: (client, r) => client.MultiTermVectors(r),
			requestAsync: (client, r) => client.MultiTermVectorsAsync(r)
		);

		protected override bool ExpectIsValid => true;
		protected override int ExpectStatusCode => 200;
		protected override HttpMethod HttpMethod => HttpMethod.POST;
		protected override string UrlPath => $"/project/_mtermvectors";

		protected override bool SupportsDeserialization => false;

		//TODO unlike mget the mtermvectors is not smart enough to omit index or type if its already specified on the path
		//not important for 2.0 release
		protected override object ExpectJson { get; } = new 
		{
			docs = Project.Projects.Select(p => new
			{
				_index = "project", _type = "project", _id = p.Name, payloads = true, field_statistics = true
			}).Take(10)
		};

		protected override Func<MultiTermVectorsDescriptor, IMultiTermVectorsRequest> Fluent => d => d
			.Index<Project>()
			.GetMany<Project>(Project.Projects.Select(p=>p.Name).Take(10), (p, i) => p.FieldStatistics().Payloads())
		;
			

		protected override MultiTermVectorsRequest Initializer => new MultiTermVectorsRequest(Index<Project>())
		{
			Documents = Project.Projects.Select(p => p.Name).Take(10)
				.Select(n=>new MultiTermVectorOperation<Project>(n) { FieldStatistics = true, Payloads = true })
		};
	}
}
