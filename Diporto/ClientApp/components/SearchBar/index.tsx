import * as React from "react";
import { Link } from 'react-router-dom';

export interface SearchButtonStyle {
  backgroundColor?: string
  color?: string
}

export interface SearchBarStyle {
  maxWidth?: number | string
}

export interface SearchProps extends React.Props<any> {
  onSearch?: (searchText: string) => void;
  buttonStyle?: SearchButtonStyle;
  barStyle?: SearchBarStyle;
  initialValue?: string
}

export interface SearchState extends React.ComponentState {
  value: string;
}

export default class SearchBar extends React.Component<SearchProps, SearchState> {
  public static defaultProps: Partial<SearchProps> = {
    buttonStyle: {
      backgroundColor: "#FE92DE",
      color: "white"
    }
  };

  constructor(props) {
    super(props)

    this.state = {value: props.initialValue == null ? '' : props.initialValue};
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  private handleChange(event) {
    this.setState({value: event.target.value});
  }

  private handleSubmit(event) {
    event.preventDefault();
    this.props.onSearch(this.state.value);
  }

  public render() {
    return (
      <form style={Object.assign({flexGrow: 1}, this.props.barStyle)} onSubmit={this.handleSubmit}>
	<input
	  className="f6 f5-l input-reset bn fl black-80 bg-white pa3 lh-solid w-100 w-75-m w-80-l br2-ns br--left-n outline-0"
	  placeholder="Search"
	  type="text"
	  name="search"
	  value={this.state.value}
	  onChange={this.handleChange}
	/>
	<input style={this.props.buttonStyle} className="f6 f5-l button-reset fl pv3 tc bn bg-animate white pointer w-100 w-25-m w-20-l br2-ns br--right-ns outline-0" type="submit" value="search"/>
      </form>
    )
  }
}
