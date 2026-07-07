import {
  Bell,
  BriefcaseBusiness,
  Building2,
  CalendarDays,
  ChevronDown,
  CircleDollarSign,
  Command,
  Home,
  MapPin,
  MessageSquare,
  Search,
  ShieldCheck,
  Sparkles,
  Users,
  Utensils
} from 'lucide-react';
import { useEffect, useMemo, useState } from 'react';
import { getHealth, type HealthResponse } from './api';

const metrics = [
  { label: 'Open jobs', value: '1,284', delta: '+12.4%', icon: BriefcaseBusiness },
  { label: 'Active workers', value: '48,910', delta: '+8.1%', icon: Users },
  { label: 'Live rooms', value: '6,732', delta: '+4.6%', icon: Building2 },
  { label: 'Meal plans', value: '2,416', delta: '+5.9%', icon: Utensils }
];

const modules = [
  { name: 'Job marketplace', count: '326 pending applications', icon: BriefcaseBusiness },
  { name: 'Worker management', count: '91 profiles need verification', icon: Users },
  { name: 'Accommodation', count: '47 room updates today', icon: Home },
  { name: 'Food services', count: '18 subscription renewals', icon: Utensils },
  { name: 'Payments', count: '13 settlements queued', icon: CircleDollarSign },
  { name: 'Notifications', count: '2 campaigns scheduled', icon: Bell }
];

const activity = [
  ['09:45', 'Worker profile verified', 'Asha R. moved to active worker status'],
  ['10:10', 'New job posted', 'Cafe part-time shift opened in Kochi'],
  ['10:22', 'Room inventory updated', 'Three shared rooms became available'],
  ['11:05', 'Payment captured', 'Premium job listing payment completed'],
  ['11:40', 'AI match generated', '42 worker recommendations queued']
];

const jobs = [
  { title: 'Evening store assistant', location: 'Kakkanad', rate: '₹180/hr', fit: 94 },
  { title: 'Cafe support staff', location: 'Edappally', rate: '₹16k/mo', fit: 88 },
  { title: 'Inventory helper', location: 'Aluva', rate: '₹750/day', fit: 82 }
];

export function App() {
  const [health, setHealth] = useState<HealthResponse | null>(null);
  const [healthError, setHealthError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();

    getHealth(controller.signal)
      .then(setHealth)
      .catch((error: unknown) => {
        setHealthError(error instanceof Error ? error.message : 'API unavailable');
      });

    return () => controller.abort();
  }, []);

  const apiState = useMemo(() => {
    if (health?.data?.databaseConnected) return 'Connected';
    if (healthError) return 'Offline';
    return 'Checking';
  }, [health, healthError]);

  return (
    <div className="shell">
      <aside className="sidebar">
        <div className="brand">
          <div className="brandMark">
            <Command size={19} />
          </div>
          <div>
            <strong>PTimeJobs</strong>
            <span>Operations Console</span>
          </div>
        </div>

        <nav className="nav">
          {[
            ['Overview', Home],
            ['Jobs', BriefcaseBusiness],
            ['Workers', Users],
            ['Accommodation', Building2],
            ['Food', Utensils],
            ['Messages', MessageSquare],
            ['Payments', CircleDollarSign],
            ['Security', ShieldCheck]
          ].map(([label, Icon], index) => (
            <button className={index === 0 ? 'navItem active' : 'navItem'} key={label as string}>
              <Icon size={18} />
              <span>{label as string}</span>
            </button>
          ))}
        </nav>
      </aside>

      <main className="main">
        <header className="topbar">
          <div className="searchBox">
            <Search size={18} />
            <input placeholder="Search workers, jobs, rooms, providers" />
          </div>
          <div className="topActions">
            <div className={`statusPill ${apiState.toLowerCase()}`}>
              <span />
              API {apiState}
            </div>
            <button className="iconButton" aria-label="Notifications">
              <Bell size={18} />
            </button>
            <button className="profileButton">
              <span>Admin</span>
              <ChevronDown size={16} />
            </button>
          </div>
        </header>

        <section className="heroPanel">
          <div className="heroCopy">
            <p className="eyebrow">Marketplace command center</p>
            <h1>Part-time work, living, food, and trust workflows in one console.</h1>
            <div className="heroActions">
              <button className="primaryButton">
                <BriefcaseBusiness size={18} />
                Review jobs
              </button>
              <button className="secondaryButton">
                <Sparkles size={18} />
                Run matching
              </button>
            </div>
          </div>
          <div className="mapVisual" aria-label="Marketplace density map">
            <div className="gridLines" />
            <div className="node n1" />
            <div className="node n2" />
            <div className="node n3" />
            <div className="node n4" />
            <div className="route r1" />
            <div className="route r2" />
            <div className="mapCard">
              <MapPin size={18} />
              <div>
                <strong>Kochi cluster</strong>
                <span>812 matches ready</span>
              </div>
            </div>
          </div>
        </section>

        <section className="metricsGrid">
          {metrics.map((metric) => (
            <article className="metricCard" key={metric.label}>
              <div className="metricIcon">
                <metric.icon size={19} />
              </div>
              <span>{metric.label}</span>
              <strong>{metric.value}</strong>
              <small>{metric.delta} this week</small>
            </article>
          ))}
        </section>

        <section className="contentGrid">
          <div className="panel modulePanel">
            <div className="panelHeader">
              <div>
                <p className="eyebrow">Modules</p>
                <h2>Operational queues</h2>
              </div>
              <button className="secondaryButton compact">
                <CalendarDays size={16} />
                Today
              </button>
            </div>
            <div className="moduleList">
              {modules.map((module) => (
                <button className="moduleRow" key={module.name}>
                  <span className="moduleIcon">
                    <module.icon size={18} />
                  </span>
                  <span>
                    <strong>{module.name}</strong>
                    <small>{module.count}</small>
                  </span>
                </button>
              ))}
            </div>
          </div>

          <div className="panel jobsPanel">
            <div className="panelHeader">
              <div>
                <p className="eyebrow">Matching</p>
                <h2>High-fit jobs</h2>
              </div>
            </div>
            <div className="jobStack">
              {jobs.map((job) => (
                <article className="jobCard" key={job.title}>
                  <div>
                    <strong>{job.title}</strong>
                    <span>{job.location} · {job.rate}</span>
                  </div>
                  <div className="fitScore">
                    <b>{job.fit}%</b>
                    <small>fit</small>
                  </div>
                </article>
              ))}
            </div>
          </div>

          <div className="panel activityPanel">
            <div className="panelHeader">
              <div>
                <p className="eyebrow">Audit</p>
                <h2>Recent activity</h2>
              </div>
            </div>
            <div className="timeline">
              {activity.map(([time, title, body]) => (
                <div className="timelineItem" key={`${time}-${title}`}>
                  <time>{time}</time>
                  <span />
                  <div>
                    <strong>{title}</strong>
                    <p>{body}</p>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </section>
      </main>
    </div>
  );
}
