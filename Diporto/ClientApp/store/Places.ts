import { fetch, addTask } from 'domain-task'
import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.
export interface PlacesState {
    filter: PlacesFilter;
	places: Place[];
	isLoading: boolean;
}

export interface PlacesFilter {
    text: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.


interface REQUEST_PLACES {
    type: 'REQUEST_PLACES',
    filter: PlacesFilter
}

interface RECEIVE_PLACES {
    type: 'RECEIVE_PLACES',
    places: Place[],
    filter: PlacesFilter
}

interface EMPTY_SEARCH_TERM {
	type: 'EMPTY_SEARCH_TERM'
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RECEIVE_PLACES | REQUEST_PLACES | EMPTY_SEARCH_TERM;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestPlaces: (filter: PlacesFilter): AppThunkAction<KnownAction> => (dispatch, getState) => {
		if (filter.text !== getState().places.filter.text) {
			let fetchTask = fetch(`/api/places?term=${filter.text}`)
			.then(response => response.json())
			.then(json => {
				return json.map(place => {
					return {
						id: place.id,
						name: place.name,
						lat: place.lat,
						lon: place.lon,
						address: place.address,
						phone: place.phone,
						photos: place.photos,
						reviews: place.reviews,
						categories: place.categories
					} as Place
				})
			})
			.then(data => {
				dispatch({ type: 'RECEIVE_PLACES', places: data, filter: filter})
			})
			.catch(error => {
				console.log(error);
			});

		addTask(fetchTask); // Ensure server-side prerendering waits for this to complete

		dispatch({ type: 'REQUEST_PLACES', filter: filter })

		}
	},
	emptySearchTerm: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
		dispatch({ type: 'EMPTY_SEARCH_TERM' })
	}
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
const defaultState: PlacesState = {
    filter: { text: '' },
	places: [],
	isLoading: false
}

export const reducer: Reducer<PlacesState> = (state: PlacesState, action: KnownAction) => {
    switch (action.type) {
	case 'RECEIVE_PLACES':
	    return {
			filter: state.filter,
			places: action.places,
			isLoading: false
	    };
	case 'REQUEST_PLACES':
	    return {
			filter: action.filter,
			places: state.places,
			isLoading: true
		};
	case 'EMPTY_SEARCH_TERM':
		return {
			filter: { text: '' },
			places: state.places,
			isLoading: state.isLoading
		}
	default:
	    // The following line guarantees that every action in the KnownAction union has been covered by a case above
	    const exhaustiveCheck: never = action;
    }

    // For unrecognized actions (or in cases where actions have no effect), must return the existing state
    //  (or default initial state if none was supplied)
    return state || defaultState;
};
