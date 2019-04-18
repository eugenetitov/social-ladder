using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using MapKit;
using UIKit;

namespace SocialLadder.iOS.Delegates
{
    public class MapViewDelegate : MKMapViewDelegate
    {
        private MKMapView _mapView;
        private CLLocationManager _clLocationManager;

        public MapViewDelegate(MKMapView mapView, CLLocationManager clLocationManager = null)
        {
            _mapView = mapView;
            _clLocationManager = clLocationManager;
        }

        public override void DidUpdateUserLocation(MKMapView mapView, MKUserLocation userLocation)
        {
            //base.DidUpdateUserLocation(mapView, userLocation);
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            //base.DidSelectAnnotationView(mapView, view);
        }
    }
}