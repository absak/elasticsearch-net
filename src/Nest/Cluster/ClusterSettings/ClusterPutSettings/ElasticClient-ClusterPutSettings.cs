﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest
{
	public partial interface IElasticClient
	{
		/// <summary>
		/// Allows to update cluster wide specific settings. Settings updated can either be persistent 
		/// (applied cross restarts) or transient (will not survive a full cluster restart). 
		/// <para> </para>http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/cluster-update-settings.html
		/// </summary>
		IClusterPutSettingsResponse ClusterPutSettings(Func<ClusterPutSettingsDescriptor, IClusterPutSettingsRequest> clusterHealthSelector = null);

		/// <inheritdoc/>
		Task<IClusterPutSettingsResponse> ClusterPutSettingsAsync(Func<ClusterPutSettingsDescriptor, IClusterPutSettingsRequest> clusterHealthSelector = null);

		/// <inheritdoc/>
		IClusterPutSettingsResponse ClusterPutSettings(IClusterPutSettingsRequest clusterSettingsRequest);

		/// <inheritdoc/>
		Task<IClusterPutSettingsResponse> ClusterPutSettingsAsync(IClusterPutSettingsRequest clusterSettingsRequest);
	}
	public partial class ElasticClient
	{
		/// <inheritdoc/>
		public IClusterPutSettingsResponse ClusterPutSettings(Func<ClusterPutSettingsDescriptor, IClusterPutSettingsRequest> selector = null) =>
			this.ClusterPutSettings(selector.InvokeOrDefault(new ClusterPutSettingsDescriptor()));

		/// <inheritdoc/>
		public Task<IClusterPutSettingsResponse> ClusterPutSettingsAsync(Func<ClusterPutSettingsDescriptor, IClusterPutSettingsRequest> selector = null) =>
			this.ClusterPutSettingsAsync(selector.InvokeOrDefault(new ClusterPutSettingsDescriptor()));

		/// <inheritdoc/>
		public IClusterPutSettingsResponse ClusterPutSettings(IClusterPutSettingsRequest clusterSettingsRequest) => 
			this.Dispatcher.Dispatch<IClusterPutSettingsRequest, ClusterSettingsRequestParameters, ClusterPutSettingsResponse>(
				clusterSettingsRequest,
				this.LowLevelDispatch.ClusterPutSettingsDispatch<ClusterPutSettingsResponse>
			);

		/// <inheritdoc/>
		public Task<IClusterPutSettingsResponse> ClusterPutSettingsAsync(IClusterPutSettingsRequest clusterSettingsRequest) => 
			this.Dispatcher.DispatchAsync<IClusterPutSettingsRequest, ClusterSettingsRequestParameters, ClusterPutSettingsResponse, IClusterPutSettingsResponse>(
				clusterSettingsRequest,
				this.LowLevelDispatch.ClusterPutSettingsDispatchAsync<ClusterPutSettingsResponse>
			);
	}
}