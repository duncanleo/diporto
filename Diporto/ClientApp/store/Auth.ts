import { fetch, addTask } from 'domain-task';
import Api from '../util/api';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

export interface AuthState {
  isFetching: boolean;
  isAuthenticated: boolean;
  user?: Credentials;
  errorMessage?: string;
}

interface LoginRequestAction {
  type: 'LOGIN_REQUEST',
  creds: Credentials
}

interface LoginSuccessAction {
  type: 'LOGIN_SUCCESS',
  idToken: string
}

interface LoginFailureAction {
  type: 'LOGIN_FAILURE',
  message: string
}

interface LogoutRequestAction {
  type: 'LOGOUT_REQUEST'
}

interface LogoutSuccessAction {
  type: 'LOGOUT_SUCCESS'
}

type KnownAction = LoginRequestAction | LoginSuccessAction | LoginFailureAction | LogoutRequestAction| LogoutSuccessAction

export const actionCreators = {
  loginUser: (creds: Credentials) : AppThunkAction<KnownAction> => (dispatch, getState) => {
    console.log("dave");
    dispatch({type: 'LOGIN_REQUEST', creds: creds})
    return Api.login(creds)
      .then(tokenResponse => {
	localStorage.setItem('id_token', tokenResponse.token);
	dispatch({type: 'LOGIN_SUCCESS', idToken: tokenResponse.token});
      }).catch(err => {
	dispatch({type: 'LOGIN_FAILURE', message: 'Bad Credentials'})
	return Promise.reject(err)
      });
  },
  logout: () : AppThunkAction<KnownAction> => (dispatch, getState) => {
    return dispatch => {
      dispatch({type: 'LOGOUT_REQUEST'})
      localStorage.removeItem('id_token')
      dispatch({type: 'LOGOUT_SUCCESS'})
    }
  }
}

const unloadedState: AuthState = { isFetching: false, isAuthenticated: false, };

export const reducer: Reducer<AuthState> = (state: AuthState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch(action.type) {
    case 'LOGIN_REQUEST':
      console.log(action);
      return Object.assign({}, state, {
	isFetching: true,
	isAuthenticated: false,
	user: action.creds
      })
    case 'LOGIN_SUCCESS':
      return Object.assign({}, state, {
	isFetching: false,
	isAuthenticated: true,
	errorMessage: ''
      })
    case 'LOGIN_FAILURE':
      return Object.assign({}, state, {
	isFetching: false,
	isAuthenticated: false,
	errorMessage: action.message
      })
    case 'LOGOUT_REQUEST':
      return Object.assign({}, state, {
	isFetching: true,
	isAuthenticated: false
      })
    case 'LOGOUT_SUCCESS':
      return Object.assign({}, state, unloadedState)
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
}
