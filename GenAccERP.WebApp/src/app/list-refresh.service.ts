import { Injectable } from '@angular/core';

export type ListModule = 'users' | 'categories' | 'items' | 'purchases' | 'sales';

/**
 * Lets list screens register a reload function (a GET call to the module's list endpoint) and lets
 * forms trigger it after a successful save. Changes are also broadcast so other open tabs re-fetch.
 */
@Injectable({ providedIn: 'root' })
export class ListRefreshService {
  private loaders = new Map<ListModule, Set<() => Promise<unknown> | void>>();
  private channel: BroadcastChannel | null =
    typeof BroadcastChannel !== 'undefined' ? new BroadcastChannel('genacc-list-refresh') : null;

  constructor() {
    if (this.channel) {
      this.channel.onmessage = (e: MessageEvent) => this.run(e.data?.modules);
    }
  }

  register(module: ListModule, loader: () => Promise<unknown> | void) {
    if (!this.loaders.has(module)) this.loaders.set(module, new Set());
    this.loaders.get(module)!.add(loader);
    return () => { this.loaders.get(module)?.delete(loader); };
  }

  /** Re-fetch the given modules (or all lists when none given) and notify other tabs. */
  async refreshList(...modules: ListModule[]) {
    this.channel?.postMessage({ modules });
    await this.run(modules);
  }

  private async run(modules?: ListModule[]) {
    const targets = modules?.length ? modules : Array.from(this.loaders.keys());
    await Promise.all(targets.flatMap(m => Array.from(this.loaders.get(m) ?? []).map(fn => fn())));
  }
}
