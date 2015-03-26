Toxiproxy.Net
=============

Project

[![Build status](https://ci.appveyor.com/api/projects/status/82gfuh999hq15sgo?svg=true)](https://ci.appveyor.com/project/mdevilliers/toxiproxy-net)

Master branch

[![Build status](https://ci.appveyor.com/api/projects/status/82gfuh999hq15sgo/branch/master?svg=true)](https://ci.appveyor.com/project/mdevilliers/toxiproxy-net/branch/master)

Test Coverage

[![Coverage Status](https://coveralls.io/repos/mdevilliers/Toxiproxy.Net/badge.svg?branch=coveralls_integration)](https://coveralls.io/r/mdevilliers/Toxiproxy.Net?branch=coveralls_integration)

Toxiproxy.Net is a .Net client for Shopify's [Toxiproxy](https://github.com/shopify/toxiproxy). Toxiproxy is a proxy to test your system by simulate network failure.


```
Toxiproxy[:mysql_master].downstream(:latency, latency: 1000).apply do
  Shop.first # this takes at least 1s
end
```

```
var connection = new Connection();
var client = connection.Proxies();
var proxy = client.FindProxy("one");
var toxics = client.FindUpStreamToxicsForProxy(proxy);
toxics.LatencyToxic.Latency = 10000;
client.UpdateUpStreamToxic(proxy, upstream.LatencyToxic);
```

```

// create a connection to toxiproxy that will call reset on disposal
// this resets the state of all the toxics and re-enables all of the proxies
using (var connection = new Connection(true)){
	
	var latency = connection.Client().FindProxy("ms_sql").Upstream.LatencyToxic;
	
	latency.Latency = 10000;
	latency.Update();

	// do your test....

} 


```

```


using (var connection = new Connection(true)){
	
	// removes the proxy - all your connections will fail
	connection.Client().FindProxy("ms_sql").Delete();
	

	// do your test....

} 


```