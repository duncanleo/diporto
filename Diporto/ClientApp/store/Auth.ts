import { fetch, addTask } from 'domain-task';
import Api from '../util/api';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';
import history from '../history';

export interface AuthState {
  isFetching: boolean;
  isAuthenticated: boolean;
  user?: User;
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

interface SetUserAction {
  type: 'SET_USER',
  user: User
}

type KnownAction = LoginRequestAction | LoginSuccessAction | LoginFailureAction | LogoutRequestAction| LogoutSuccessAction | SetUserAction

export const actionCreators = {
  loginUser: (creds: Credentials) : AppThunkAction<KnownAction> => (dispatch, getState) => {
    dispatch({type: 'LOGIN_REQUEST', creds: creds})
    return Api.login(creds)
      .then(tokenResponse => {
        localStorage.setItem('id_token', tokenResponse.token);
        dispatch({type: 'LOGIN_SUCCESS', idToken: tokenResponse.token});
        return tokenResponse.token;
      })
      .then(token => {
        Api.getMe(token)
          .then(user => {
            localStorage.setItem('user', JSON.stringify(user));
            dispatch({type: 'SET_USER', user: user})
            history.replace('/');
          });
      })
      .catch(err => {
        dispatch({type: 'LOGIN_FAILURE', message: 'Bad Credentials'})
        return Promise.reject(err)
      });
  },
  logout: () : AppThunkAction<KnownAction> => (dispatch, getState) => {
    dispatch({type: 'LOGOUT_REQUEST'});
    localStorage.removeItem('id_token');
    localStorage.removeItem('user');
    dispatch({type: 'LOGOUT_SUCCESS'});
  },
  signupUser: (creds: UserRegistration,
    onFinish: (creds: Credentials) => AppThunkAction<KnownAction>) : AppThunkAction<KnownAction> => (dispatch, getState) => {
    return Api.signUp(creds)
      .then(response => {
        onFinish({username: creds.UserName, password: creds.Password});
      })
      .catch(error => {
        alert(error);
      })
  }
}

export const unloadedState: AuthState = {
  isFetching: false,
  isAuthenticated: localStorage.getItem('id_token') ? true : false,
  user: localStorage.getItem('user') ? JSON.parse(localStorage.getItem('user')) : null
};

export const reducer: Reducer<AuthState> = (state: AuthState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch(action.type) {
    case 'LOGIN_REQUEST':
      return Object.assign({}, state, {
        isFetching: true,
        isAuthenticated: false,
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
    case 'SET_USER':
      return Object.assign({}, state, {
        user: action.user
      })
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
}
