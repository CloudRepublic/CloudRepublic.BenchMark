<template>
  <div class="main-content bg-default">
    <!-- Navbar -->
    <base-nav
      class="navbar-top navbar-horizontal navbar-dark"
      containerClasses="px-4 container"
      expand
    >
      <router-link slot="brand" class="navbar-brand" to="/">
        <img src="img/brand/logo-white.png">
      </router-link>

      <template v-slot="{ closeMenu }">
        <!-- Collapse header -->
        <div class="navbar-collapse-header d-md-none">
          <div class="row">
            <div class="col-6 collapse-brand">
              <router-link to="/">
                <img src="img/brand/logo-white.png">
              </router-link>
            </div>
            <div class="col-6 collapse-close">
              <button
                type="button"
                @click="closeMenu"
                class="navbar-toggler"
                aria-label="Toggle sidenav"
              >
                <span></span>
                <span></span>
              </button>
            </div>
          </div>
        </div>
        <!-- Navbar items -->
        <ul class="navbar-nav ml-auto">
          <li class="nav-item">
            <router-link class="nav-link nav-link-icon" to="/">
              <span class="nav-link-inner--text">Serverless Benchmark</span>
            </router-link>
          </li>
          <li class="nav-item">
            <a
              class="nav-link nav-link-icon"
              href="https://github.com/CloudRepublic/CloudRepublic.BenchMark"
              target="_blank"
            >
              <span class="nav-link-inner--text">Source Code</span>
            </a>
          </li>
          <li class="nav-item">
            <a
              class="nav-link nav-link-icon"
              href="https://cloudrepublic.github.io/"
              target="_blank"
            >
              <span class="nav-link-inner--text">Blog</span>
            </a>
          </li>
        </ul>
      </template>
    </base-nav>
    <!-- Header -->
    <div class="header bg-gradient-success py-7 py-lg-8">
      <div class="container pt-9 pb-5"></div>
      <div class="separator separator-bottom separator-skew zindex-100">
        <svg
          x="0"
          y="0"
          viewBox="0 0 2560 100"
          preserveAspectRatio="none"
          version="1.1"
          xmlns="http://www.w3.org/2000/svg"
        >
          <polygon class="fill-default" points="2560 0 2560 100 0 100"></polygon>
        </svg>
      </div>
    </div>
    <!-- Page content -->
    <div class="container pb-5" style="margin-top: -22rem;">
      <slide-y-up-transition mode="out-in" origin="center top">
        <router-view></router-view>
      </slide-y-up-transition>
    </div>
    <footer class="py-5">
      <div class="container">
        <div class="row align-items-center justify-content-xl-between">
          <div class="col-xl-6">
            <div class="copyright text-center text-xl-left text-muted">
              &copy; {{ year }}
              <a
                href="https://www.cloudrepublic.nl/"
                class="font-weight-bold ml-1"
                target="_blank"
              >Cloud Republic</a>
            </div>
          </div>
          <div class="col-xl-6">
            <ul class="nav nav-footer justify-content-center justify-content-xl-end">
              <li class="nav-item">
                <a
                  href="https://www.cloudrepublic.nl/"
                  class="nav-link"
                  target="_blank"
                >Cloud Republic</a>
              </li>

              <li class="nav-item">
                <a href="https://cloudrepublic.github.io/" class="nav-link" target="_blank">Blog</a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>
<script>
import { SlideYUpTransition } from 'vue2-transitions';

export default {
  name: 'benchmark-layout',
  components: {
    SlideYUpTransition
  },
  data() {
    return {
      year: new Date().getFullYear(),
      showMenu: false
    };
  }
};
</script>
<style></style>
