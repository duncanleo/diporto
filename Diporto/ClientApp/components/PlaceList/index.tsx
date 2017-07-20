import * as React from 'react';
import PlaceListItem from '../PlaceListItem';

export interface PlaceListProps extends React.Props<any> {
  places: Place[]
}
export interface PlaceListState extends React.ComponentState {}

export default class PlaceList extends React.Component<PlaceListProps, PlaceListState> {
  public static defaultProps: Partial<PlaceListProps> = {
    places: []
  }

  private renderPlaceList() {
    return this.props.places.map(place => {
      return <PlaceListItem key={place.id} place={place}/>
    })
  }

  public render() {
    return (
      <div>
	{this.renderPlaceList()}
      </div>
    )
  }
}
