var map;
var clicked = 0;
var zoom = 7;
var lat = 29.062559;
var lon = 31.098543;
var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
var cntrposition = new OpenLayers.LonLat(lon, lat).transform(fromProjection, toProjection);

function InitiatMap() {
    ;
    if (clicked == 0) {
        map = new OpenLayers.Map("Map", {
            controls: [new OpenLayers.Control.PanZoomBar(), new OpenLayers.Control.LayerSwitcher({}), new OpenLayers.Control.Permalink(),
            new OpenLayers.Control.MousePosition({}), new OpenLayers.Control.ScaleLine(), new OpenLayers.Control.OverviewMap(), ]
        });
        var mapnik = new OpenLayers.Layer.OSM("MAP");
        var markers = new OpenLayers.Layer.Markers("Markers");
        map.addLayers([mapnik, markers]);
        map.addLayer(mapnik);
        //هنا بادي الماركر خط الطول والعرض
        map.setCenter(cntrposition, zoom);
        //هنا بتحط الماركر في الخريطة
        markers.addMarker(new OpenLayers.Marker(cntrposition));
        var click = new OpenLayers.Control.Click();
        map.addControl(click);
        click.activate();
    }
    else {
        var mapnik = new OpenLayers.Layer.OSM("MAP");
        var markers = new OpenLayers.Layer.Markers("Markers");
        map.addLayers([mapnik, markers]);
        map.addLayer(mapnik);
        map.setCenter(cntrposition, 18);
        //هنا بتحط الماركر في الخريطة
        markers.addMarker(new OpenLayers.Marker(cntrposition));
        var click = new OpenLayers.Control.Click();
        map.addControl(click);
        click.activate();
    }
};

OpenLayers.Control.Click = OpenLayers.Class(OpenLayers.Control, {
    defaultHandlerOptions: {
        'single': true,
        'double': false,
        'pixelTolerance': 0,
        'stopSingle': false,
        'stopDouble': false
    },
    initialize: function (options) {

        this.handlerOptions = OpenLayers.Util.extend({}, this.defaultHandlerOptions);
        OpenLayers.Control.prototype.initialize.apply(this, arguments);

        this.handler = new OpenLayers.Handler.Click(this, {
            'click': this.trigger
        }, this.handlerOptions);
    },
    trigger: function (e) {
        var lonlat = map.getLonLatFromPixel(e.xy);
        lonlat1 = new OpenLayers.LonLat(lonlat.lon, lonlat.lat).transform(toProjection, fromProjection);

        $("#Longitude").val(lonlat1.lon);
        $("#Latitude").val(lonlat1.lat);
        var markers = new OpenLayers.Layer.Markers("Markers");
        cntrposition = new OpenLayers.LonLat(lonlat1.lon, lonlat1.lat).transform(fromProjection, toProjection);
        markers.addMarker(new OpenLayers.Marker(cntrposition));
        var click = new OpenLayers.Control.Click();
        map.addControl(click);
        click.activate();
        map.destroy()
        clicked = 0;
        InitiatMap();
        // alert("Hello..." + lonlat1.lon + "  " + lonlat1.lat);
    }
});

function addmarker(lat, lon) {
    clicked = 1;
    cntrposition = new OpenLayers.LonLat(lon, lat).transform(fromProjection, toProjection);
    InitiatMap();
}