Toxiproxy.Net
=============

Toxiproxy.Net is a .Net client for Shopify's [Toxiproxy](https://github.com/shopify/toxiproxy) REST/JSON Api. Toxiproxy is a proxy to test your system by simulating network failure.


Project

[![Build status](https://ci.appveyor.com/api/projects/status/82gfuh999hq15sgo?svg=true)](https://ci.appveyor.com/project/mdevilliers/toxiproxy-net)

Master branch

[![Build status](https://ci.appveyor.com/api/projects/status/82gfuh999hq15sgo/branch/master?svg=true)](https://ci.appveyor.com/project/mdevilliers/toxiproxy-net/branch/master)

Test Coverage

[![Coverage Status](https://coveralls.io/repos/mdevilliers/Toxiproxy.Net/badge.svg?branch=master)](https://coveralls.io/r/mdevilliers/Toxiproxy.Net?branch=master)

Getting started
---------------

A copy of toxiproxy compiled for windows is in the [compiled folder](https://github.com/mdevilliers/Toxiproxy.Net/tree/master/compiled/Win64). Linux and Darwin builds are available from the [official release site](https://github.com/Shopify/toxiproxy/releases)

Usage
-----

The unit tests give a good overview of how to fully use the api. Here are some examples to get started -

Set up a proxy

```
var connection = new Connection();
var client = connection.Client();

var localToGoogleProxy = new Proxy() { Name = "localToGoogle", Enabled = true, Listen = "127.0.0.1:44399", Upstream = "google.com:443" };

client.Add(localToGoogleProxy);

```


```
// assuming the proxy is running with its default settings e.g. localhost:8474
// and there is a connection defined with the name "ms_sql"
// create a connection to toxiproxy that will call reset on disposal
// this resets the state of all the toxics and re-enables all of the proxies
using (var connection = new Connection(true))
{

    // find the upstream latency toxic
    var latency = connection.Client().FindProxy("ms_sql").UpStreams().LatencyToxic;

    // set a latency of 1 second 
    latency.Latency = 1000;

    // save it
    latency.Update();

    // do your test....

} 

```
