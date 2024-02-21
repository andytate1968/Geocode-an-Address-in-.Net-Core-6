using Newtonsoft.Json;
using RestSharp;

var client = new RestClient("https://geocode.search.hereapi.com");
var request = new RestRequest("v1/geocode?q=84404+Njjdd+Dunes+Trl+McKinney+TX+75070&apiKey=jnddtDKi68lNy75MGM9bjx4LGnKpR74x_cVYd8o8Wi8", Method.Get);
request.AddHeader("User-Agent", "Nothing");
var geoResult = client.Execute(request).Content;
var root = JsonConvert.DeserializeObject<HereGeoCode.Root>(geoResult);

var result = new GeoCode.AddressValidation();

if (root == null || root.items.Count == 0)
{
    result.isSuspect = true;
    result.geoLat = 0.00;
    result.geoLon = 0.00;
}
else
{
    var item = root.items[0];
    result.geoLat = item.position.lat;
    result.geoLon = item.position.lng;
    if (item.resultType == "houseNumber")
    {       
        result.isSuspect = false;
    }
    else if (item.resultType == "street")
    {
        if (item.scoring.fieldScore.streets[0] < 0.5)
        {
            result.isSuspect = true;
        }
        else
        {
            result.isSuspect = false;
        }
    }
    else {
        result.isSuspect = true;
    }
}

