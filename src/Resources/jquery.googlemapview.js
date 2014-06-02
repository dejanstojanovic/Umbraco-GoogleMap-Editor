/* VIEWER */

$.fn.GoogleMapViewer = function (options) {
    var defaults = {
        mapLanguage: "en",
        editMode: false,
        Zoom: 13,
        Center: {
            Latitude: 25.0417,
            Longitude: 55.2194
        },
        mapSettings: {
            "Width": 800,
            "Height": 400,
            "Locations": []
        }
    };
    var settings = $.extend({}, defaults, options);
    var mapApiUrl = "//maps.googleapis.com/maps/api/js?sensor=false&callback=mapApiLoaded&libraries=drawing,places";

    var selector = $(this);

    if ((typeof google !== "undefined" && google !== null ? google.maps : void 0) == null) {
        $.getScript(mapApiUrl);

        window.mapApiLoaded = function () {
            selector.each(function (index) {
                var container = selector.get(index);
                initializeGoogleMapViewer(container);
            });
        }
    }
    else {
        selector.each(function (index) {
            var container = selector.get(index);
            initializeGoogleMapViewer(container);
        });
    }

    function initializeGoogleMapViewer(container) {

        if ($(container).html()==null || $(container).html() == "") {
            return false;
        }

        settings.mapSettings = $.parseJSON($(container).html());
        $(container).html(null);
        $(container).show();

        map = new google.maps.Map(container, {
            center: new google.maps.LatLng(settings.Center.Latitude, settings.Center.Longitude),
            zoom: settings.mapSettings.Zoom,
            zoomControl: true,
            panControl: true,
            scaleControl: true,
            streetViewControl: true,
            infoWindow: null
        });

        map.container = container;
        map.locations = settings.mapSettings.Locations;

        $(container).attr("data-rendered", "true");

        google.maps.event.addListenerOnce(map, 'idle', function () {
            if (map.locations != null && map.locations.length > 0) {
                for (i = 0; i < map.locations.length; i++) {
                    initMapObject(map, map.locations[i]);
                }
            }
        });
    }

    function initMapObject(map, location) {
        switch (location.LocationType) {
            case google.maps.drawing.OverlayType.MARKER:

                var marker = new google.maps.Marker({
                    draggable: settings.editMode,
                    map: map,
                    position: new google.maps.LatLng(location.Coordinates[0].Latitude, location.Coordinates[0].Longitude),
                    Location: location
                });
                if (location.Icon != null && location.Icon != "") {
                    marker.setIcon(new google.maps.MarkerImage(location.Icon));
                }

                location.Overlay = marker;
                attachLocationHandlers(map, location, google.maps.drawing.OverlayType.MARKER);

                break;
            case google.maps.drawing.OverlayType.CIRCLE:
                var circle = new google.maps.Circle({
                    draggable: settings.editMode,
                    map: map,
                    strokeWeight: location.BorderWeight,
                    //fillOpacity: settings.mouseOutOpacity,
                    strokeColor: location.BorderColor,
                    fillColor: location.FillColor,
                    map: map,
                    radius: location.Radius,
                    center: new google.maps.LatLng(location.Coordinates[0].Latitude, location.Coordinates[0].Longitude),
                    Location: location
                });
                location.Overlay = circle;
                attachLocationHandlers(map, location, google.maps.drawing.OverlayType.CIRCLE);
                break;
            case google.maps.drawing.OverlayType.POLYLINE:
                var polylineCorners = [];
                for (c = 0; c < location.Coordinates.length; c++) {
                    var polylineCorner = new google.maps.LatLng(location.Coordinates[c].Latitude, location.Coordinates[c].Longitude)
                    polylineCorners.push(polylineCorner);
                }

                polyline = new google.maps.Polyline({
                    path: polylineCorners,
                    strokeWeight: location.BorderWeight,
                    strokeColor: location.BorderColor,
                    draggable: settings.editMode,
                    Location: location
                });
                polyline.setMap(map);
                location.Overlay = polyline;
                attachLocationHandlers(map, location, google.maps.drawing.OverlayType.POLYLINE);
                break;
            case google.maps.drawing.OverlayType.POLYGON:
                var poligonCorners = [];
                for (c = 0; c < location.Coordinates.length; c++) {
                    var polygonCorner = new google.maps.LatLng(location.Coordinates[c].Latitude, location.Coordinates[c].Longitude)
                    poligonCorners.push(polygonCorner);
                }
                polygon = new google.maps.Polygon({
                    path: poligonCorners,
                    strokeWeight: location.BorderWeight,
                    //fillOpacity: settings.mouseOutOpacity,
                    strokeColor: location.BorderColor,
                    fillColor: location.FillColor,
                    draggable: settings.editMode,
                    Location: location
                });
                polygon.setMap(map);
                location.Overlay = polygon;
                attachLocationHandlers(map, location, google.maps.drawing.OverlayType.POLYGON);
                break;
            case google.maps.drawing.OverlayType.RECTANGLE:
                var rectangle = new google.maps.Rectangle({
                    draggable: settings.editMode,
                    map: map,
                    strokeWeight: location.BorderWeight,
                    //fillOpacity: settings.mouseOutOpacity,
                    strokeColor: location.BorderColor,
                    fillColor: location.FillColor,
                    map: map,
                    bounds: new google.maps.LatLngBounds(
                          new google.maps.LatLng(location.Coordinates[1].Latitude, location.Coordinates[1].Longitude),
                          new google.maps.LatLng(location.Coordinates[0].Latitude, location.Coordinates[0].Longitude)),
                    Location: location
                });
                location.Overlay = rectangle;
                attachLocationHandlers(map, location, google.maps.drawing.OverlayType.RECTANGLE);
                break;
        }

    }

    function attachLocationHandlers(map, location, type) {
        google.maps.event.addListener(location.Overlay, 'click', function () {
            if (map.infoWindow != null) {
                map.infoWindow.close();
                map.activeLocation = null;
            }
            map.infoWindow = new google.maps.InfoWindow();
            map.activeLocation = location;

            if (location.Message == null || location.Message == "") {
                return false;
            }

            map.infoWindow.setContent(location.Message);

            var position = null;
            switch (type) {
                case google.maps.drawing.OverlayType.MARKER:
                    position = location.Overlay.position;
                    break;
                case google.maps.drawing.OverlayType.CIRCLE:
                    position = location.Overlay.getCenter();
                    break;
                case google.maps.drawing.OverlayType.POLYLINE:
                    position = getPolyLineCenter(location.Overlay);
                    break;
                case google.maps.drawing.OverlayType.POLYGON:
                    position = getPolygonCenter(location.Overlay);
                    break;
                case google.maps.drawing.OverlayType.RECTANGLE:
                    position = location.Overlay.getBounds().getCenter();
                    break;
            }

            if (position != null && type != google.maps.drawing.OverlayType.MARKER) {
                map.infoWindow.setPosition(position);
                map.infoWindow.open(map);
            }
            else {
                map.infoWindow.open(map, location.Overlay);
            }
        });
    }

    function getPolygonCenter(polygon) {
        var bounds = new google.maps.LatLngBounds();

        for (i = 0; i < polygon.getPath().getLength() ; i++) {
            bounds.extend(new google.maps.LatLng(polygon.getPath().getAt(i).lat(),
                                                 polygon.getPath().getAt(i).lng()));
        }
        return bounds.getCenter();
    }
}