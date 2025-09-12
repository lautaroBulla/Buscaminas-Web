import { FetchOptions } from 'ofetch'

declare module '#app' {
  interface NuxtApp {
    $apiFetch<T = any>(request: string, options?: FetchOptions): Promise<T>
  }
}

export {}

