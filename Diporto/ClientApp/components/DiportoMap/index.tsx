import * as React from 'react';
import ReactMapboxGl from 'react-mapbox-gl';

const Map = ReactMapboxGl({accessToken: "pk.eyJ1IjoianVydmlzIiwiYSI6ImNqNTh5MmlnNzAya3EzMXE3eGFhMWtrczQifQ.BgT8aHg52oKkki4GV8oNpA"})

const DiportoMap: React.SFC<any> = props =>
  <Map>
    {props.children}
  </Map>

export default DiportoMap;