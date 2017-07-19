import * as React from "react";

export interface SearchProps extends React.Props<any> {
  onSearch: (searchText: string) => void;
}

export interface SearchState extends React.ComponentState {
  value: string;
}

export default class SearchBar extends React.Component<SearchProps, SearchState> {
  constructor(props) {
    super(props)

    this.state = {value: ''};
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  private handleChange(event) {
    this.setState({value: event.target.value});
  }

  private handleSubmit(event) {
    this.props.onSearch(this.state.value);
    event.preventDefault();
  }

  public render() {
    return (
      <form style={{flexGrow: 1}} onSubmit={this.handleSubmit}>
	<input
	  className="f6 f5-l input-reset bn fl black-80 bg-white pa3 lh-solid w-100 w-75-m w-80-l br2-ns br--left-n"
	  placeholder="Search"
	  type="text"
	  name="search"
	  value={this.state.value}
	  onChange={this.handleChange}
	/>
	<input className="f6 f5-l button-reset fl pv3 tc bn bg-animate bg-black-70 hover-bg-black white pointer w-100 w-25-m w-20-l br2-ns br--right-ns" type="submit" value="search"/>
      </form>
    )
  }
}
