using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using MapKit;
using SocialLadder.iOS.Delegates;
using UIKit;

namespace SocialLadder.iOS.CustomControlls
{
    public class MapView
    {
        private CLLocationManager _clLocationManager;
        private MapViewDelegate _mapDelegate;
        private MKMapView _mapView;

        public MapView(MKMapView mapView, double latitude, double longitude)
        {
            _mapView = mapView;
            InitMap();
            AddAnnotation(latitude, longitude);
        }

        public void InitMap()
        {
            _clLocationManager = new CLLocationManager();

            _clLocationManager.RequestAlwaysAuthorization();
            _clLocationManager.RequestWhenInUseAuthorization();

            _mapView.ShowsUserLocation = true;
            _mapView.ScrollEnabled = _mapView.ZoomEnabled = true;
            _mapView.ShowsCompass = true;

            _mapDelegate = new MapViewDelegate(_mapView, _clLocationManager)
            {

            };

            _mapView.Delegate = _mapDelegate;
        }
        private void AddAnnotation(double latitude, double longitude)
        {
            _mapView.AddAnnotations(new MKPointAnnotation()
            {
                Title = "MyAnnotation",
                Coordinate = new CLLocationCoordinate2D(latitude, longitude)
            });
        }
    }

}