import * as React from 'react';
import PlaceListItem from '../PlaceListItem';
import Map from '../Map';

export interface PlaceListProps extends React.Props<any> {
  places: Place[]
}
export interface PlaceListState extends React.ComponentState {}

export default class PlaceList extends React.Component<PlaceListProps, PlaceListState> {
  public static defaultProps: Partial<PlaceListProps> = {
    places: []
  }

  private renderPlaceList() {
    return this.props.places.map((place, index) => {
      return <PlaceListItem key={place.id} place={place} index={index}/>
    })
  }

  public render() {
    return (
      <div className="flex mw8 center">
	<div className="w-70">
	  {this.renderPlaceList()}
	</div>
	<div className="w-10"></div>
	<div className="w-20 mt4">
	  <Map width={200} height={200} viewport={{latitude: 1.03, longitude: 103.5, zoom: 8}}/>
	</div>
      </div>
    )
  }
}
