using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;

namespace SocialLadder.Droid.Helpers
{
    public static class GoogleMapHelper
    {
        public static void UpdateMapZoom(GoogleMap _map, double lat, double lon, double? radius)
        {
            var location = new LatLng(lat, lon);
            AddMarker(_map, location);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(GetZoomLevel(radius.Value));
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            _map.MoveCamera(cameraUpdate);

            double currentRadius = radius ?? 100.0;
            if (radius > 6000000)
            {
                radius = 6000000;
            }
            _map.AddCircle(new CircleOptions().InvokeCenter(location).InvokeRadius(currentRadius).InvokeStrokeWidth(0f).InvokeFillColor(Color.Argb(50, 50, 50, 200)));
        }

        private static int GetZoomLevel(double radius)
        {
            double scale = radius / 500;
            if (scale <= 1)
            {
                return 14;
            }
            var zoomLevel = (int)(15 - (Math.Log(scale) / Math.Log(2)));
            return zoomLevel;
        }

        private static void AddMarker(GoogleMap _map, LatLng location)
        {
            _map.Clear();
            var icon = BitmapDescriptorFactory.FromResource(Resource.Drawable.map_marker_icon);
            _map.AddMarker(new MarkerOptions().SetPosition(location).Draggable(true).SetAlpha(0.7f).SetIcon(icon));
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            builder.Include(location);
            LatLngBounds bounds = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngBounds(bounds, 40, 40, 3);
            _map.AnimateCamera(cameraUpdate);
        }
    }
}