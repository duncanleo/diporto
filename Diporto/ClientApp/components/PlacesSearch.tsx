import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as PlacesState from '../store/Places';

type PlacesSearchProps =
  PlacesState.PlacesState
  & typeof PlacesState.actionCreators
  & RouteComponentProps<{ searchTerm: string }>;

class PlacesSearch extends React.Component<PlacesSearchProps, {}> {
  componentWillMount() {
    let searchTerm = this.props.match.params.searchTerm
    this.props.requestPlaces({ text: searchTerm });
  }

  componentWillReceiveProps(nextProps: PlacesSearchProps) {
    let searchTerm = nextProps.match.params.searchTerm;
    this.props.requestPlaces({ text: searchTerm });
  }

  public render() {
    return <div>
      <h1>Places</h1>
      { this.renderPlacesTable() }
    </div>
  }

  private renderPlacesTable() {
    return <table>
      <thead>
	<td>Name</td>
	<td>lat</td>
	<td>lon</td>
	<td>phone</td>
	<td>address</td>
      </thead>
      <tbody>
	 {this.props.places.map( place =>
	  <tr>
	    <td>{ place.name }</td>
	    <td>{ place.lat }</td>
	    <td>{ place.lon } </td>
	    <td>{ place.phone }</td>
	    <td>{ place.address }</td>
	  </tr>
	)}
      </tbody>
    </table>
  }
}

export default connect(
    (state: ApplicationState) => state.places, // Selects which state properties are merged into the component's props
    PlacesState.actionCreators                 // Selects which action creators are merged into the component's props
)(PlacesSearch) as typeof PlacesSearch;