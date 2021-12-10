/* tslint:disable */
/* eslint-disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { Cerveza } from '../models/cerveza';
import { Data } from '../models/data';

@Injectable({
  providedIn: 'root',
})
export class MinimalApiService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation get
   */
  static readonly GetPath = '/';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `get()` instead.
   *
   * This method doesn't expect any request body.
   */
  get$Response(params?: {
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.GetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `get$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  get(params?: {
  }): Observable<string> {

    return this.get$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * Path part for operation holaGet
   */
  static readonly HolaGetPath = '/hola';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `holaGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  holaGet$Response(params: {
    name: string;
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.HolaGetPath, 'get');
    if (params) {
      rb.query('name', params.name, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `holaGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  holaGet(params: {
    name: string;
  }): Observable<string> {

    return this.holaGet$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * Path part for operation holaNewNameSurnameGet
   */
  static readonly HolaNewNameSurnameGetPath = '/holaNew/{name}/{surname}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `holaNewNameSurnameGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  holaNewNameSurnameGet$Response(params: {
    name: string;
    surname: string;
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.HolaNewNameSurnameGetPath, 'get');
    if (params) {
      rb.path('name', params.name, {});
      rb.path('surname', params.surname, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `holaNewNameSurnameGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  holaNewNameSurnameGet(params: {
    name: string;
    surname: string;
  }): Observable<string> {

    return this.holaNewNameSurnameGet$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * Path part for operation peticionGet
   */
  static readonly PeticionGetPath = '/peticion';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `peticionGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  peticionGet$Response(params?: {
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.PeticionGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `peticionGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  peticionGet(params?: {
  }): Observable<string> {

    return this.peticionGet$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * Path part for operation cervezasGet
   */
  static readonly CervezasGetPath = '/cervezas';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `cervezasGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  cervezasGet$Response(params?: {
  }): Observable<StrictHttpResponse<Array<Cerveza>>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.CervezasGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<Cerveza>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `cervezasGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  cervezasGet(params?: {
  }): Observable<Array<Cerveza>> {

    return this.cervezasGet$Response(params).pipe(
      map((r: StrictHttpResponse<Array<Cerveza>>) => r.body as Array<Cerveza>)
    );
  }

  /**
   * Path part for operation cervezaIdGet
   */
  static readonly CervezaIdGetPath = '/cerveza/{id}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `cervezaIdGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  cervezaIdGet$Response(params: {
    id: number;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.CervezaIdGetPath, 'get');
    if (params) {
      rb.path('id', params.id, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `cervezaIdGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  cervezaIdGet(params: {
    id: number;
  }): Observable<void> {

    return this.cervezaIdGet$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation pruebaGet
   */
  static readonly PruebaGetPath = '/prueba';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `pruebaGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  pruebaGet$Response(params?: {
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.PruebaGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `pruebaGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  pruebaGet(params?: {
  }): Observable<string> {

    return this.pruebaGet$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * Path part for operation postPost
   */
  static readonly PostPostPath = '/post';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `postPost()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  postPost$Response(params: {
    body: Data
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, MinimalApiService.PostPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/json');
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `postPost$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  postPost(params: {
    body: Data
  }): Observable<string> {

    return this.postPost$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

}
