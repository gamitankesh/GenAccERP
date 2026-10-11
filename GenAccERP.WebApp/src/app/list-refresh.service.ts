import { Injectable, OnDestroy } from '@angular/core';

export type ListModule = 'users' | 'categories' | 'items' | 'purchases' | 'sales';

/**
 * Lets list components register a reload function (a GET call to the module's list endpoint)
 * and lets forms trigger it after a successful create/update/delete. Changes are also broadcast
 * so lists open in other browser tabs re-fetch from the server.
 */
@Injectable({ providedIn: 'root' })
export class ListRefreshService implements OnDestroy {
  private loaders = new Map<ListModule, Set<() => Promise<unknown> | unknown>>();
  private channel: BroadcastChannel | null =
    typeof BroadcastChannel !== 'undefined' ? new BroadcastChannel('genacc-list-refresh') : null;

  constructor() {
    if (this.channel) this.channel.onmessage = (e: MessageEvent) => this.run(e.data as ListModule[]);
  }

  /** Register a list reload function (also fetches the latest data now); returns an unregister function. */
  register(module: ListModule, loader: () => Promise<unknown> | unknown) {
    if (!this.loaders.has(module)) this.loaders.set(module, new Set());
    this.loaders.get(module)!.add(loader);
    void loader();
    return () => { this.loaders.get(module)?.delete(loader); };
  }

  /** Re-fetch the given modules' lists (or all lists when none given), here and in other tabs. */
  refreshList(...modules: ListModule[]) {
    const targets = modules.length ? modules : [...this.loaders.keys()];
    this.channel?.postMessage(targets);
    return this.run(targets);
  }

  private run(modules: ListModule[]) {
    const calls: unknown[] = [];
    for (const m of new Set(modules)) this.loaders.get(m)?.forEach(fn => calls.push(fn()));
    return Promise.all(calls);
  }

  ngOnDestroy() { this.channel?.close(); }
}
